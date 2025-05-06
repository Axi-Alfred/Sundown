using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
    [Header("Bounciness Settings")]
    public float uppåtkraft = 10f;      // Hur högt clownen studsar
    public float maxSidokraft = 5f;     // Maximal kraft åt sidan

    private void OnCollisionEnter2D(Collision2D kollision)
    {
        GameObject objektSomTräffade = kollision.gameObject;

        if (objektSomTräffade.CompareTag("Clown"))
        {
            Rigidbody2D clownKropp = objektSomTräffade.GetComponent<Rigidbody2D>();

            if (clownKropp != null)
            {
                // Nollställ fallhastigheten i y-led
                Vector2 nuvarandeHastighet = clownKropp.velocity;
                nuvarandeHastighet.y = 0f;
                clownKropp.velocity = nuvarandeHastighet;

                // Räkna ut var clownen träffade trampolinen
                Vector3 clownPosition = clownKropp.transform.position;
                Vector3 trampolinPosition = transform.position;
                float skillnadIXLed = clownPosition.x - trampolinPosition.x;

                // Bestäm sidkraft beroende på träffposition
                float sidKraft = Mathf.Clamp(skillnadIXLed, -1f, 1f) * maxSidokraft;

                // Skapa kraftvektor
                Vector2 studsKraft = new Vector2(sidKraft, uppåtkraft);

                // Applicera kraft
                clownKropp.AddForce(studsKraft, ForceMode2D.Impulse);
            }
        }
    }
}
