using System.Collections;
using System.Collections.Generic;
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

            // Spel fryser
            Time.timeScale = 0;

            // Scenbyte function här nere
            GameManager1.EndTurn();
            // SceneManager.LoadScene("GameOverScene");
        }

    }

    public bool IsGameOver()
    {
        return gameOver;
    }


}
