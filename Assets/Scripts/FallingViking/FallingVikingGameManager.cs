using TMPro;
using UnityEngine;

public class FallingVikingGameManager : MonoBehaviour
{
    public static FallingVikingGameManager Instance;

    public int score = 0;
    public int scoreToWin = 10;
    public TextMeshProUGUI scoreText;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        if (gameEnded) return;

        score += points;

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (score >= scoreToWin)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        Debug.Log("🛡️ You win! Enough vikings caught.");
        PlayerData.currentPlayerTurn.AddScore(1);
        GameManager1.EndRound();
    }
}
