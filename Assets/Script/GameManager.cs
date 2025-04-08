using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Setup")]
    public GameObject letterTilePrefab;
    public Transform letterParent;

    [Header("Words")]
    [SerializeField]
    private List<string> wordList = new() {
        "apple", "hello", "world", "dream", "mirror", "flip", "level", "cloud", "right", "brain"
    };

    private int currentWordIndex = 0;
    private int totalToFix = 0;
    private int fixedCount = 0;

    void Awake() => Instance = this;

    void Start()
    {
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
        GenerateWord(nextWord);
    }

    public void GenerateWord(string word)
    {
        // Clean up old tiles
        foreach (Transform child in letterParent)
            Destroy(child.gameObject);

        fixedCount = 0;
        totalToFix = 0;

        System.Random rand = new();
        char[] nonFlippableLetters = { 'l', 'o', 'x', 's', 'z', 'i'};
        List<int> flippedIndices = new();
        List<int> flippableIndices = new(); // ✅ used to enforce min flipped
        List<LetterTile> spawnedTiles = new();

        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            string correct = c.ToString();

            bool isFlippable = !nonFlippableLetters.Contains(char.ToLower(c));
            bool startsCorrect = rand.NextDouble() < 0.4f;

            // Prevent flipping non-flippable letters
            if (!isFlippable)
                startsCorrect = true;

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
        }

        // ✅ Enforce minimum 3 flipped (wrong) letters
        while (flippedIndices.Count < 3)
        {
            var unflipped = flippableIndices
                .Where(index => !flippedIndices.Contains(index))
                .ToList();

            if (unflipped.Count == 0) break; // no more to flip safely

            int idx = unflipped[rand.Next(unflipped.Count)];
            flippedIndices.Add(idx);

            var targetTile = spawnedTiles[idx];
            targetTile.ForceFlip();

            totalToFix++;
        }
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
        Debug.Log("❌ Wrong letter tapped: " + tile.correctLetter);
    }

    void WinGame()
    {
        Debug.Log("🏆 You fixed the word: " + wordList[currentWordIndex]);
        currentWordIndex++;
        Invoke(nameof(LoadNextWord), 1.5f); // Delay for feedback
    }
}




