using UnityEngine;

public class BalloonGameManager : MonoBehaviour
{
    private int hiddenScore = 0;
    public int scoreNeededToWin = 20;

    public void BalloonPopped()
    {
        hiddenScore++;
    }

    public void CheckWinCondition()
    {
        if (hiddenScore >= scoreNeededToWin)
        {
            Debug.Log("✅ You Win!");
            PlayerData.currentPlayerTurn.AddScore(1);
        }
        else
        {
            Debug.Log("❌ You Lose!");
        }

        // Stop spawning balloons after the game ends
        BalloonSpawner spawner = FindObjectOfType<BalloonSpawner>();
        if (spawner) spawner.StopAllCoroutines();

        GameManager1.EndTurn();
    }
}
