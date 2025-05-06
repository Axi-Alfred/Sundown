using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
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