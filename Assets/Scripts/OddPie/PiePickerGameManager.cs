using System.Collections.Generic;
using UnityEngine;

public class PiePickerGameManager : MonoBehaviour
{
    [Header("Pie Setup")]
    [Tooltip("The prefab used to spawn each pie")]
    public GameObject piePrefab;

    [Tooltip("The parent transform for pie layout (e.g. Grid or Horizontal Layout Group)")]
    public Transform pieParent;

    [Tooltip("Sprites considered correct (normal pies)")]
    public List<Sprite> goodPies;

    [Tooltip("Sprites considered incorrect (the odd one to find)")]
    public List<Sprite> badPies;

    [Header("Game Settings")]
    [Tooltip("How many correct rounds needed to win")]
    public int roundsToWin = 3;

    [Tooltip("Number of pies shown per round")]
    public int piesPerRound = 5;

    private int currentRound = 0;
    private bool gameOver = false;

    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        ClearOldPies();

        if (currentRound >= roundsToWin)
        {
            WinGame();
            return;
        }

        currentRound++;

        int oddIndex = Random.Range(0, piesPerRound);

        for (int i = 0; i < piesPerRound; i++)
        {
            GameObject pieGO = Instantiate(piePrefab, pieParent);
            ClickablePie pieScript = pieGO.GetComponent<ClickablePie>();

            bool isOdd = (i == oddIndex);

            Sprite chosenSprite = isOdd
                ? badPies[Random.Range(0, badPies.Count)]
                : goodPies[Random.Range(0, goodPies.Count)];

            pieScript.Setup(chosenSprite, isOdd, this);
        }

        Debug.Log($"🥧 Round {currentRound}/{roundsToWin} started. Tap the odd pie!");
    }

    public void HandlePieTapped(bool wasOdd)
    {
        if (gameOver) return;

        if (wasOdd)
        {
            Debug.Log("✅ Correct pie tapped.");
            StartRound();
        }
        else
        {
            Debug.Log("❌ Wrong pie! You lose.");
            LoseGame();
        }
    }

    void WinGame()
    {
        gameOver = true;
        Debug.Log("🏆 You win! Point awarded.");
        PlayerData.currentPlayerTurn.AddScore(1);
        GameManager1.EndTurn();
    }

    void LoseGame()
    {
        gameOver = true;
        Debug.Log("💀 You lose.");
        GameManager1.EndTurn();
    }

    void ClearOldPies()
    {
        foreach (Transform child in pieParent)
        {
            Destroy(child.gameObject);
        }
    }
}
