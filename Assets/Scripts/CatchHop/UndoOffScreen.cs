<<<<<<< HEAD:Assets/Scripts/CatchHop/UndoOffScreen.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    public float offScreenX = 15f; // Justera detta tillräckligt långt till höger
    public float offScreenY = -10f; // Justera detta om clowner faller ner också

    void Update()
    {
        // Om clownen är för långt åt höger eller för långt ner → förstör
        if (transform.position.x > offScreenX || transform.position.y < offScreenY)
        {
            Destroy(gameObject);
        }
    }

}
=======
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
>>>>>>> Ermias:Assets/CatchHop/Scripts/UndoOffScreen.cs
