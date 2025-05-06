<<<<<<< HEAD:Assets/Scripts/CatchHop/TrampolineBounce.cs
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> Ermias:Assets/CatchHop/Scripts/TrampolineBounce.cs
using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/CatchHop/TrampolineBounce.cs
    [Header("Bounciness Settings")]
    public float upwardForce = 10f;  // Hur mycket uppåt
    public float forwardForce = 5f;  // Hur mycket framåt

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Clown"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Nollställ vertikal hastighet
                rb.velocity = new Vector2(rb.velocity.x, 0);

                // Skapa en riktning uppåt och lite åt sidan (höger)
                Vector2 bounceDirection = new Vector2(1f, 1f).normalized;

                // Använd kraft med justerbara värden
                Vector2 finalForce = new Vector2(bounceDirection.x * forwardForce, bounceDirection.y * upwardForce);
                rb.AddForce(finalForce, ForceMode2D.Impulse);
            }
        }
    }
}
=======
    // Dessa värden styr hur mycket clownen studsar uppåt och åt sidan
    public float uppåtkraft = 10f;      // Hur högt clownen studsar
    public float maxSidokraft = 5f;     // Maximal kraft åt sidan

    private void OnCollisionEnter2D(Collision2D kollision)
    {
        // Kontrollera om det som träffar trampolinen är en clown
        GameObject objektSomTräffade = kollision.gameObject;

        if (objektSomTräffade.CompareTag("Clown"))
        {
            // Hämta clownens Rigidbody2D
            Rigidbody2D clownKropp = objektSomTräffade.GetComponent<Rigidbody2D>();

            // Kontrollera att Rigidbody2D finns
            if (clownKropp != null)
            {
                // Nollställ fallhastigheten (bara y-led)
                Vector2 nuvarandeHastighet = clownKropp.velocity;
                nuvarandeHastighet.y = 0f;
                clownKropp.velocity = nuvarandeHastighet;

                // Räkna ut var clownen träffade trampolinen
                Vector3 clownPosition = clownKropp.transform.position;
                Vector3 trampolinPosition = transform.position;

                float skillnadIXLed = clownPosition.x - trampolinPosition.x;

                // Bestäm hur mycket kraft som ska ges åt sidan
                float sidKraft = 0f;

                // Om clownen är till vänster om mitten
                if (skillnadIXLed < 0f)
                {
                    sidKraft = -maxSidokraft;
                }
                // Om clownen är till höger om mitten
                else if (skillnadIXLed > 0f)
                {
                    sidKraft = maxSidokraft;
                }
                // Om clownen är exakt i mitten
                else
                {
                    sidKraft = 0f;
                }

                // Skapa kraftvektor
                Vector2 studsKraft = new Vector2(sidKraft, uppåtkraft);

                // Applicera kraften
                clownKropp.AddForce(studsKraft, ForceMode2D.Impulse);
            }
        }
    }
}
>>>>>>> Ermias:Assets/CatchHop/Scripts/TrampolineBounce.cs
