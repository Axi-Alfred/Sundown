using UnityEngine;
using UnityEngine.SceneManagement;

public class ClownElopeManager : MonoBehaviour
{
    private bool gameOver = false;

    public int maxEscaped = 10;
    private int escapedCount = 0;

    public void ObjectEscaped()
    {
        if (gameOver) return;

        escapedCount++;
        Debug.Log("Objects escaped: " + escapedCount);

        if (escapedCount >= maxEscaped)
        {
            gameOver = true;
            Debug.Log("🎯 GAME OVER!");

            // Pause game
            Time.timeScale = 1;

            // End turn or load next scene
            GameManager1.EndTurn();
            // Or: SceneManager.LoadScene("GameOverScene");
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
