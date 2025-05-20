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
    [SerializeField]
    private List<string> wordList = new() {
        "apple", "hello", "world", "dream", "mirror", "flip", "level", "cloud", "right", "brain"
    };

    private int currentMistakes = 0;
    private bool gameOver = false;
    private int displayedMaxMistakes;
    private int currentWordIndex = 0;
    private int totalToFix = 0;
    private int fixedCount = 0;
    private int RightWords = 0;

    private List<NewLetter> allTiles = new();
    private List<NewLetter> spawnedTiles = new();
    private List<int> flippedIndices;
    private List<int> flippableIndices;

    public static isItRight Instance;
    public readonly Color victoryGreen = new Color(0.2f, 0.8f, 0.2f, 1f);
    public GameObject letterTilePrefab;
    public Transform letterParent;

    void Awake() => Instance = this;

    void Start()
    {
        audioPool = FindObjectOfType<AudioPool>();
        audioPool.PlayBackgroundMusic(backgroundMusic, 0.5f);
        currentMistakes = 0;
        UpdateMistakeUI();
        LoadNextWord();
    }

    void LoadNextWord()
    {
        if (RightWords >= 3)
        {
            PlayerManager.Instance.currentPlayerTurn.AddScore(1);
            GameManager1.EndTurn();
            return;
        }

        if (currentWordIndex >= wordList.Count)
        {
            Debug.Log("🎉 All words complete!");
            GameManager1.EndTurn(); // fallback
            return;
        }

        string nextWord = wordList[currentWordIndex];
        Debug.Log("🔤 Starting word: " + nextWord);

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
        foreach (NewLetter tile in spawnedTiles)
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
            string shown = startsCorrect ? correct : NewLetter.FlipLetter(correct);
            bool isCorrect = isFlippable && !startsCorrect;

            if (isCorrect)
            {
                flippedIndices.Add(i);
                totalToFix++;
            }

            if (isFlippable)
                flippableIndices.Add(i);

            GameObject go = Instantiate(letterTilePrefab, letterParent);
            NewLetter tile = go.GetComponent<NewLetter>();
            tile.Setup(shown, correct, isCorrect, startsCorrect);
            go.GetComponent<Button>().onClick.AddListener(tile.OnClick);

            spawnedTiles.Add(tile);
            allTiles.Add(tile);
        }
    }

    private void EnsureMinimumFlips(int wordLength)
    {
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
        UpdateMistakeUI();
    }

    public void OnCorrectLetterTapped(NewLetter tile)
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

    public void OnWrongLetterTapped(NewLetter tile)
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

    private void UpdateMistakeUI()
    {
        mistakeText.text = $"Mistakes: {currentMistakes}/{maxMistakes}";
    }

    public void ShakeMistakeText()
    {
        mistakeText.transform.DOShakePosition(
            duration: 0.4f,
            strength: new Vector3(10f, 0f, 0f),
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

        foreach (NewLetter tile in allTiles)
        {
            tile.SetVictoryColor(victoryGreen);
            tile.StartCoroutine(tile.Bounce());
            yield return new WaitForSeconds(delay);
        }

        RightWords++; // ✅ Increment correct word count
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
            yield return new WaitForSeconds(flashDelay);
        }

        yield return new WaitForSeconds(0.4f);

        foreach (var tile in allTiles)
        {
            tile.StartCoroutine(tile.FadeToColor(Color.gray));
        }

        yield return new WaitForSeconds(0.4f);

        GameManager1.EndTurn(); // ✅ Return to Wheel after loss
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
    }
}
