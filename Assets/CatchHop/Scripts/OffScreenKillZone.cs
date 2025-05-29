using UnityEngine;

public class OffScreenKillZone : MonoBehaviour
{
    public float rightLimit = 15f;      // X-position där objekt förstörs (långt till höger)
    public float lowerLimit = -10f;     // Y-position där objekt förstörs (långt ner)

    private static int crashedClownCount = 0; // Statisk räknare som gäller för alla clowner
    private static int maxCrashes = 20;       // Max antal clowner som får krascha

    void Update()
    {
        // Kontrollera om objektet har lämnat skärmen
        bool isBeyondRight = transform.position.x > rightLimit;
        bool isBelowBottom = transform.position.y < lowerLimit;

        if (isBeyondRight || isBelowBottom)
        {
            // Om detta objekt är en clown, öka räknaren
            if (gameObject.CompareTag("Clown"))
            {
                crashedClownCount = crashedClownCount + 1;

                // Om tillräckligt många clowner kraschat, stoppa spelet
                if (crashedClownCount >= maxCrashes)
                {
                    Debug.Log("Spelet är slut – 20 clowner har kraschat.");
                    Time.timeScale = 0f; // Stoppa spelet
                }
            }

            // Förstör objektet oavsett vad det är
            Destroy(gameObject);
        }
    }
}