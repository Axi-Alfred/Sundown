using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class isItRight : MonoBehaviour
{
    // Det här är min game manager för kortspelet. Har som syfte att presentera ord till spelaren som spelaren sen kommer att behöva rätta, eftersom orden är upp och ned/flippade. OBS roterar inte orden

    [Header("Misstagshantering")]
    [SerializeField] private int maxMistakes = 3;
    [SerializeField] private TMP_Text mistakeText;

    [Header("Ljud")]
    [SerializeField] private AudioClip correct, incorrect, posAll, negAll;
    [SerializeField] private AudioClip spawnPop;
    [SerializeField] private AudioPool audioPool;

    [Header("Visuella element")]
    [SerializeField] private Transform bounceTarget;
    [SerializeField] private TMP_Text introText;

    // Variabler för spelinformation
    private int currentMistakes = 0;
    private int currentWordIndex = 0;
    private int totalToFix = 0;
    private int fixedCount = 0;
    private int rightWords = 0;

    private bool gameOver = false;

    // ordHantering. Tiles = varje bokstav. Indices = platsen som självaste bokstaven har i ett ord. T.ex. Bokstaven "o" har index 2 i ordet "Clown"
    private List<NewLetter> allTiles = new();
    private List<NewLetter> spawnedTiles = new(); // Tiles som visas just nu
    private List<int> flippedIndices; // Tiles som startas som flipped (inkorrekt)
    private List<int> flippableIndices;
    private List<string> wordList = new() { // Lista på ord som vi behöver fixa
        "clown", "pies", "unicycle", "circus", "cake", "ball", "happy", "confetti", "balloon", "juggling", "gymnastics", "magician", "acrobat", "elephant", "show", "carnival", "popcorn", "jester"
    };

    public static isItRight Instance;
    public readonly Color victoryGreen = new Color(0.2f, 0.8f, 0.2f, 1f);
    public GameObject letterTilePrefab;
    public Transform letterParent;

    void Awake() => Instance = this; // Gör att isItRight kan kommas åt globalt av andra scripts. Mest för NewLetter
    void Start()
    {
        audioPool = FindObjectOfType<AudioPool>();
        currentMistakes = 0;

        // Blandar ordlistan
        if (wordList != null && wordList.Count > 0)
            wordList = wordList.OrderBy(x => Random.value).ToList(); 

        UpdateMistakeUI();
        LoadNextWord();
        if (introText != null)
        {
            // fade-in och fade-out effekt för introText (hinten)
            introText.alpha = 0;
            introText.DOFade(1f, 1f).SetEase(Ease.InOutSine) // fade in
                     .OnComplete(() =>
                     {
                         introText.DOFade(0f, 1f).SetDelay(8f); // wait 2 sec, fade out
                     });
        }

    }

    void LoadNextWord()
    {
        if (rightWords >= 3) // Om spelaren svarar korrekt på 3 ord tar spelet slut
        {
            FindObjectOfType<StarBurstDOTween>().TriggerBurst();
            PlayerData.currentPlayerTurn.AddScore(1);
            GameManager1.EndTurn();
            return;
        }

        if (currentWordIndex >= wordList.Count) // Om spelaren får slut på ord tar spelet slut
        {
            PlayerData.currentPlayerTurn.AddScore(1);
            Debug.Log("🎉 All words complete!");
            GameManager1.EndTurn();
            return;
        }

        // Ladda nästa ord och återställ

        string nextWord = wordList[currentWordIndex];
        Debug.Log("🔤 Starting word: " + nextWord);

        int length = nextWord.Length;
        currentMistakes = 0;
        UpdateMistakeUI();
        GenerateWord(nextWord);
        currentWordIndex++;
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
        // ta sönder alla gamla ordtiles och rensa listan
        allTiles.Clear();
        foreach (NewLetter tile in spawnedTiles)
        {
            Destroy(tile.gameObject);
        }
        spawnedTiles.Clear();
    }

    private void InitState()
    {
        // starta om variablerna när vi laddar ett nytt ord
        fixedCount = 0;
        totalToFix = 0;
        currentMistakes = 0;

        flippedIndices = new();
        flippableIndices = new();
    }

    private void CreateTiles(string word)
    {
        // Skapas en tile för varje bokstav i ett ord
        System.Random rand = new();
        HashSet<char> nonFlippableLetters = new() // Symmetriska bokstäver eller svårlästa bokstäver som alltid ska vara korrekta
    {
        'l', 'o', 'x', 's', 'z', 'i', 'u', 'n', 'h',
        'O', 'X', 'S', 'Z', 'I', 'U', 'N', 'H', 'V', 'C'
    };

        for (int i = 0; i < word.Length; i++) // Loppar igenom varje bokstav i ett ord och kontrollerar om det går att flippa det
        {
            char letterChar = word[i];
            string correct = letterChar.ToString();
            bool isFlippable = !nonFlippableLetters.Contains(letterChar);

            // 40% chans att en bokstav är korrekt från början
            bool startsCorrect = !isFlippable || rand.NextDouble() < 0.4f;

            // Om bokstaven är korrekt från början, visa bokstaven, annars visa den upp och ner/flippad
            // Om ordet inte kan flippas eller är inkorrekt, visa ett blanksteg
            string shown = startsCorrect
                ? correct
                : (isFlippable ? correct : "");

            bool isCorrect = isFlippable && !startsCorrect; // Ett ord är korrekt om det går att flippa och startar som inkorrekt

            if (isCorrect) // Räkna anttalet rättade ord och hur många som behöver fixas
            {
                flippedIndices.Add(i);
                totalToFix++;
            }

            if (isFlippable) // håller koll på vilka ord som kan flippas
                flippableIndices.Add(i);

            GameObject go = Instantiate(letterTilePrefab, letterParent);
            NewLetter tile = go.GetComponent<NewLetter>();

            tile.Setup(shown, correct, isCorrect, startsCorrect);
            tile.transform.localScale = Vector3.zero;

            tile.transform.DOScale(1f, 0.4f) // orden skalas upp och ned när dom spawnar in
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.05f)
                .OnStart(() =>
                {
                    audioPool.PlaySound(spawnPop, 0.8f, Random.Range(0.95f, 1.1f));
                });



            // Rotera endast inkorrekta bokstäver
            tile.contentTransform.localRotation = startsCorrect || !isFlippable
                ? Quaternion.identity
                : Quaternion.Euler(0, 0, 180);

            // Inaktivera att redan rättade bokstäver kan klickas på igen
            Button button = go.GetComponent<Button>();
            if (string.IsNullOrEmpty(shown))
            {
                button.interactable = false;
            }
            else
            {
                button.onClick.AddListener(tile.OnClick);
                button.interactable = true;
            }

            spawnedTiles.Add(tile);
            allTiles.Add(tile);
        }
    }
    private void EnsureMinimumFlips(int wordLength)
    {
        // Det ska finnas minst 2 eller 3 bokstäver som behöver fixas
        int minMistakesAllowed = 2;
        int desiredFlips = Mathf.Clamp(flippableIndices.Count, minMistakesAllowed, 3);

        System.Random rand = new();

        while (flippedIndices.Count < desiredFlips) // Flippa randomiserat med bokstäver om det inte finns tillräckligt med inkorrekta bokstäver
        {
            var unflipped = flippableIndices.Except(flippedIndices).ToList(); // Except ger mig alla bokstäver som går att fixas förutom dom som inte redan är flippade, så vi kan flippa dom
            if (unflipped.Count == 0) break;

            int idx = unflipped[rand.Next(unflipped.Count)];
            flippedIndices.Add(idx);

            var tile = spawnedTiles[idx];
            tile.ForceFlip();
            tile.isCorrect = true; // denna tile behöver bli fixad
        }

        totalToFix = flippedIndices.Count;

        // maxMistakes ska vara minst 3, om det finns två misstag så kan maxMistakes vara 2
        maxMistakes = totalToFix >= 3 ? 3 : minMistakesAllowed;
    }


    private void FinalizeWordSetup()
    {
        UpdateMistakeUI();
    }

    public void OnCorrectLetterTapped(NewLetter tile) // När ett ord har blivit rättat
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
        float delayStep = 0.05f;

        for (int i = 0; i < spawnedTiles.Count; i++)
        {
            // Apply uniform green and only jump
            spawnedTiles[i].SetVictoryColor(victoryGreen);
            StartCoroutine(spawnedTiles[i].VictoryJump(i * delayStep));
        }

        rightWords++;
        currentWordIndex++;
        currentMistakes = 0;
        gameOver = false;

        yield return new WaitForSeconds(spawnedTiles.Count * delayStep + 0.5f);
        LoadNextWord();
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

        GameManager1.EndTurn();
    }
    public void CorrectPop()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
    }

    void WinGame()
    {
        Debug.Log("🏆You fixed the word: " + wordList[currentWordIndex]);
        StartCoroutine(PlayVictorySequence());
    }

    private void LoseGame()
    {
        gameOver = true;
        Debug.Log("💀 You lost!");
        StartCoroutine(PlayLoseSequence());
    }

}
