using UnityEngine;

public class ClownElopeManager : MonoBehaviour
{
    private bool gameOver = false; // Indikerar om spelet är över

    public int maxEscaped = 10; // Max antal flyende objekt innan game over
    private int escapedCount = 0; // Räknare för flyende objekt

    // Hanterar när ett objekt flyr
    public void ObjectEscaped()
    {
        if (gameOver) return;

        escapedCount++;
        Debug.Log("Antal flyende objekt: " + escapedCount);

        if (escapedCount >= maxEscaped)
        {
            gameOver = true;
            Debug.Log("🎯 SPELLET SLUT!");
            
            // Fryser spelet
            Time.timeScale = 0;
        }
    }

    // Kontrollerar om spelet är över
    public bool IsGameOver()
    {
        return gameOver;
    }
}