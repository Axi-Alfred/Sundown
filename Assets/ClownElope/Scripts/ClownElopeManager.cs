using UnityEngine;

// Skript som hanterar spelets logik
public class ClownElopeManager : MonoBehaviour
{
    // Variabel som indikerar om spelet är över
    private bool gameOver = false;

    // Variabler för räddade och förlorade clowner
    public int maxEscaped = 10;              // Max antal flyende clowner innan game over
    private int escapedCount = 0;            // Räknare för flyende clowner
    public int savedCount = 0;               // Räknare för räddade clowner
    public int targetSavedClowns = 20;       // Antal clowner som behövs för att vinna

    // Metod som kallas när en clown har flytt
    public void ObjectEscaped()
    {
        // Om spelet redan är över, gör ingenting
        if (gameOver == true)
        {
            return;
        }

        // Öka räknaren för flyende clowner
        escapedCount = escapedCount + 1;

        // Skriv ut till konsolen
        Debug.Log("Antal flyende clowner: " + escapedCount);

        // Kontrollera om för många clowner har flytt
        if (escapedCount >= maxEscaped)
        {
            gameOver = true;
            Debug.Log("💥 SPELET ÖVER! För många clowner har flytt.");
            Time.timeScale = 0f;
        }
    }

    // Metod som kallas när en clown har räddats
    public void ObjectSaved()
    {
        // Om spelet redan är över, gör ingenting
        if (gameOver == true)
        {
            return;
        }

        // Öka räknaren för räddade clowner
        savedCount = savedCount + 1;

        // Skriv ut till konsolen
        Debug.Log("Antal räddade clowner: " + savedCount);

        // Kontrollera om spelaren har räddat tillräckligt många clowner för att vinna
        if (savedCount >= targetSavedClowns)
        {
            gameOver = true;
            Debug.Log("🎉 GRATTIS! Du har räddat tillräckligt många clowner!");
            Time.timeScale = 0f;
        }
    }

    // Metod som returnerar om spelet är över
    public bool IsGameOver()
    {
        return gameOver;
    }
}