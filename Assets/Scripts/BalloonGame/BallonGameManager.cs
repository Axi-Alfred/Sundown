using UnityEngine;

public class BalloonGameManager : MonoBehaviour
{
    private int hiddenScore = 0;
    public int scoreNeededToWin = 20;

    // Called each time a balloon pops
    public void BalloonPopped()
    {
        hiddenScore++;
    }

    // Called by your TimeManager when time runs out
    public void CheckWinCondition()
    {
        if (hiddenScore >= scoreNeededToWin)
        {
            Debug.Log("🎉 You Win! 🎉");
        }
        else
        {
            Debug.Log("💥 You Lose! 💥");
        }

        // Stop spawning balloons after the game ends
        BalloonSpawner spawner = FindObjectOfType<BalloonSpawner>();
        if (spawner) spawner.StopAllCoroutines();
    }
}
