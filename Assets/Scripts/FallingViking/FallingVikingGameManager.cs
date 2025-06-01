using TMPro;
using UnityEngine;

public class FallingVikingGameManager : MonoBehaviour
{
    public static FallingVikingGameManager Instance;

    public int vikingsCaught = 0;
    public int scoreToWin = 10;
    public TextMeshProUGUI scoreText;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {vikingsCaught}";

    }

    public void OnVikingCaught()
    {
        if (gameEnded) return;

        vikingsCaught++;

        if (scoreText != null)
            scoreText.text = $"Score: {vikingsCaught}";

        if (vikingsCaught >= scoreToWin)
            WinGame();
    }

    public void OnAxeCaught()
    {
        if (gameEnded) return;

        vikingsCaught = Mathf.Max(0, vikingsCaught - 2); // No negative score

        if (scoreText != null)
            scoreText.text = $"Score: {vikingsCaught}";
    }

    private void WinGame()
    {
        gameEnded = true;

        Debug.Log("🛡️ You win! Enough vikings caught.");
        FindObjectOfType<StarBurstDOTween>().TriggerBurst();
        PlayerData.currentPlayerTurn.AddScore(1);
        GameManager1.EndTurn();
    }

}
