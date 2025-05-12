using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class isItRight : MonoBehaviour
{
    [SerializeField] private int maxMistakes = 2;
    [SerializeField] private TMP_Text mistakeText;
    [SerializeField] private AudioClip correct, incorrect, posAll, negAll;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioPool audioPool;
    [SerializeField] private List<string> wordList = new() {
        "apple", "hello", "world", "dream", "mirror", "flip", "level", "cloud", "right", "brain"
    };
    private int currentMistakes = 0;
    private bool gameOver = false;
    private int displayedMaxMistakes;
    private int currentWordIndex = 0;
    private int totalToFix = 0;
    private int fixedCount = 0;

    private List<LetterTile> allTiles = new();
    private List<LetterTile> spawnedTiles = new();
    private List<int> flippedIndices;
    private List<int> flippableIndices;



    public static isItRight Instance;
    public readonly Color victoryGreen = new Color(0.2f, 0.8f, 0.2f, 1f); // nice, strong green
    public GameObject letterTilePrefab;
    public Transform letterParent;




    void Awake() => Instance = this;

    void Start()
    {
        audioPool = FindObjectOfType<AudioPool>();
        audioPool.PlayBackgroundMusic(backgroundMusic, 0.5f); // Adjust volume as needed
        currentMistakes = 0;
        UpdateMistakeUI();
        LoadNextWord();
    }
    void LoadNextWord()
    {
        if (currentWordIndex >= wordList.Count)
        {
            Debug.Log("🎉 All words complete!");
            PlayerData.currentPlayerTurn.AddScore(1);
            GameManager1.EndTurn();
            return;
        }


        string nextWord = wordList[currentWordIndex];
        Debug.Log("🔤 Starting word: " + nextWord);

        // 🔢 Dynamically set allowed mistakes based on word length
        int length = nextWord.Length;

        currentMistakes = 0;
        UpdateMistakeUI();

        GenerateWord(nextWord);
    }
    public void GenerateWord(string word)
    {
        CleanupOldTiles();

        InitState();

        CreateTiles(word);

        EnsureMinimumFlips(word.Length);

        FinalizeWordSetup();
    }
    private void CleanupOldTiles()
    {
        allTiles.Clear();
        foreach (LetterTile tile in spawnedTiles)
        {
            Destroy(tile.gameObject);
        }
        spawnedTiles.Clear();
    }
    private void InitState()
    {
        fixedCount = 0;
        totalToFix = 0;
        currentMistakes = 0;

        flippedIndices = new();
        flippableIndices = new();
    }

    private void CreateTiles(string word)
    {
        System.Random rand = new();
        HashSet<string> nonFlippableLetters = new() { "l", "o", "x", "s", "z", "i", " ", "" };
        for (int i = 0; i < word.Length; i++)
        {
            string correct = word[i].ToString();
            bool isFlippable = !nonFlippableLetters.Contains(correct);
            bool startsCorrect = !isFlippable || rand.NextDouble() < 0.4f;
            string shown = startsCorrect ? correct : LetterTile.FlipLetter(correct);
            bool isCorrect = isFlippable && !startsCorrect;

            if (isCorrect)
            {
                flippedIndices.Add(i);
                totalToFix++;
            }

            if (isFlippable)
                flippableIndices.Add(i);

            GameObject go = Instantiate(letterTilePrefab, letterParent);
            LetterTile tile = go.GetComponent<LetterTile>();
            tile.Setup(shown, correct, isCorrect, startsCorrect);
            go.GetComponent<Button>().onClick.AddListener(tile.OnClick);

            spawnedTiles.Add(tile);
            allTiles.Add(tile);
        }
    }
    private void EnsureMinimumFlips(int wordLength)
    {
        // Sätt minsta antal bokstäver som måste vara fel:
        int distinctLetters = spawnedTiles.Select(t => t.correctLetter).Distinct().Count();
        int minFlipped = distinctLetters > 5 ? 3 : Mathf.Min(2, wordLength);

        System.Random rand = new();

        while (flippedIndices.Count < minFlipped)
        {
            var unflipped = flippableIndices.Except(flippedIndices).ToList();
            if (unflipped.Count == 0) break;

            int idx = unflipped[rand.Next(unflipped.Count)];
            flippedIndices.Add(idx);

            var tile = spawnedTiles[idx];
            tile.ForceFlip();
            tile.isCorrect = true;
        }

        totalToFix = spawnedTiles.Count(t => t.isCorrect);
        displayedMaxMistakes = totalToFix;
        maxMistakes = distinctLetters > 5 ? 3 : 2;
    }

    private void FinalizeWordSetup()
    {
        // Don't touch displayedMaxMistakes here
        UpdateMistakeUI();
    }

    public void OnCorrectLetterTapped(LetterTile tile)
    {
        Debug.Log("✅ Correct letter tapped: " + tile.correctLetter);
        audioPool.PlaySound(correct, 2f, Random.Range(0.8f, 1.2f));
        fixedCount++;

        if (fixedCount >= totalToFix)
        {
            Debug.Log("🎉 Word fixed!");
            WinGame();
        }
    }
    public void OnWrongLetterTapped(LetterTile tile)
    {
        if (gameOver) return;
        audioPool.PlaySound(incorrect, 2f, Random.Range(0.8f, 1.2f));
        currentMistakes++;
        UpdateMistakeUI();

        Debug.Log($"❌ Mistake {currentMistakes}/{maxMistakes}");
        ShakeMistakeText();

        if (currentMistakes >= maxMistakes)
        {
            LoseGame();
        }

    }
    /*public void StartMistakeTextFloat()
    {
        Vector3 originalPos = mistakeText.transform.localPosition;

        Sequence floatSeq = DOTween.Sequence();

        floatSeq.Append(
            mistakeText.transform.DOLocalMove(originalPos + new Vector3(10f, 10f, 0f), 1f)
        );
        floatSeq.Append(
            mistakeText.transform.DOLocalMove(originalPos + new Vector3(-10f, -10f, 0f), 1f)
        );
        floatSeq.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }*/

    private void UpdateMistakeUI()
    {
        mistakeText.text = $"Mistakes: {currentMistakes}/{maxMistakes}";
    }
    public void ShakeMistakeText()
    {
        mistakeText.transform.DOShakePosition(
            duration: 0.4f,
            strength: new Vector3(10f, 0f, 0f), // shake only sideways
            vibrato: 10,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );
    }


    private IEnumerator PlayVictorySequence()
    {
        audioPool.PlaySound(posAll, 2f, Random.Range(0.9f, 1f));
        float delay = 0.05f;
        foreach (LetterTile tile in allTiles)
        {
            tile.SetVictoryColor(victoryGreen);
            tile.StartCoroutine(tile.Bounce()); // 🟢 studsa i ordning
            yield return new WaitForSeconds(delay);
        }

        currentWordIndex++;
        currentMistakes = 0;
        gameOver = false;

        Invoke(nameof(LoadNextWord), 1.5f);
    }
    private IEnumerator PlayLoseSequence()
    {
        audioPool.PlaySound(negAll, 2f, Random.Range(0.9f, 1f));
        float flashDelay = 0.08f;
        foreach (var tile in allTiles)
        {
            tile.GetComponent<Button>().interactable = false;
            StartCoroutine(tile.AnimateShake());
            yield return new WaitForSeconds(flashDelay); // left to right
        }

        yield return new WaitForSeconds(0.4f); // wait before greying out

        foreach (var tile in allTiles)
        {
            tile.StartCoroutine(tile.FadeToColor(Color.gray));
        }

        yield return new WaitForSeconds(0.4f);
        
    }

    void WinGame()
    {
        Debug.Log("🏆 You fixed the word: " + wordList[currentWordIndex]);
        StartCoroutine(PlayVictorySequence());
    }
    private void LoseGame()
    {
        gameOver = true;
        Debug.Log("💀 You lost!");

        StartCoroutine(PlayLoseSequence());


        // Du kan lägga till mer: ljud, grafik, restart-knapp etc.
    }

}
