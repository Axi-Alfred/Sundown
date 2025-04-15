using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class isItRight : MonoBehaviour
{
    [SerializeField] private int maxMistakes = 3;
    [SerializeField] private TMP_Text mistakeText;
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
        currentMistakes = 0;
        UpdateMistakeUI();

        LoadNextWord();
    }
    void LoadNextWord()
    {
        if (currentWordIndex >= wordList.Count)
        {
            Debug.Log("🎉 All words complete!");
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
        HashSet<string> nonFlippableLetters = new() { "l", "o", "x", "s", "z", "i", "" };
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
        int minFlipped = Mathf.Min(3, wordLength);
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
    }
    private void FinalizeWordSetup()
    {
        displayedMaxMistakes = totalToFix < 3 ? totalToFix : 3;
        maxMistakes = 3;
        UpdateMistakeUI();
    }


    public void OnCorrectLetterTapped(LetterTile tile)
    {
        Debug.Log("✅ Correct letter tapped: " + tile.correctLetter);
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

        currentMistakes++;
        UpdateMistakeUI();

        Debug.Log($"❌ Mistake {currentMistakes}/{maxMistakes}");

        currentMistakes++;

        if (currentMistakes >= 3)
        {
            LoseGame();
        }

    }
    private void UpdateMistakeUI()
    {
        mistakeText.text = $"Mistakes: {currentMistakes}/{displayedMaxMistakes}";
    }

    void WinGame()
    {
        Debug.Log("🏆 You fixed the word: " + wordList[currentWordIndex]);
        StartCoroutine(PlayVictorySequence());
    }
    private IEnumerator PlayVictorySequence()
    {
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
    private void LoseGame()
    {
        gameOver = true;
        Debug.Log("💀 You lost!");

        foreach (var tile in allTiles)
        {
            tile.GetComponent<Button>().interactable = false;
            tile.GetComponent<Image>().color = Color.gray;
        }

        // Du kan lägga till mer: ljud, grafik, restart-knapp etc.
    }

}
