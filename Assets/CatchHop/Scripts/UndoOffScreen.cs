using UnityEngine;

public class UndoOffScreen : MonoBehaviour
{
    public float högerGräns = 15f;       // X-position där objekt förstörs (långt till höger)
    public float nedreGräns = -10f;      // Y-position där objekt förstörs (långt ner)

    private static int antalKraschadeClowner = 0; // Statisk räknare som gäller för alla clowner
    private static int maxKrascher = 20;          // Max antal clowner som får krascha

    void Update()
    {
        // Kontrollera om objektet har lämnat skärmen
        bool ärUtanförHöger = transform.position.x > högerGräns;
        bool ärUtanförNedre = transform.position.y < nedreGräns;

        if (ärUtanförHöger || ärUtanförNedre)
        {
            // Om detta objekt är en clown, öka räknaren
            if (gameObject.CompareTag("Clown"))
            {
                antalKraschadeClowner = antalKraschadeClowner + 1;

                // Om tillräckligt många clowner kraschat, stoppa spelet
                if (antalKraschadeClowner >= maxKrascher)
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