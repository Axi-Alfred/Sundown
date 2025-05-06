using UnityEngine;

public class PoolCounter : MonoBehaviour
{
    public int maxAntalClowner = 10; // Max antal clowner som får landa i poolen
    private int nuvarandeAntalClowner = 0; // Räknare för landade clowner

    void OnCollisionEnter2D(Collision2D kollision)
    {
        GameObject objekt = kollision.gameObject;

        if (objekt.CompareTag("Clown"))
        {
            ContactPoint2D[] kontaktpunkter = kollision.contacts;

            if (kontaktpunkter.Length > 0)
            {
                Vector2 kontaktpunkt = kontaktpunkter[0].point;
                Vector2 poolPosition = transform.position;

                // Kontrollera att clownen landade uppifrån
                if (kontaktpunkt.y > poolPosition.y)
                {
                    // Ta bort clownen
                    Destroy(objekt);

                    // Öka räknaren
                    nuvarandeAntalClowner = nuvarandeAntalClowner + 1;

                    // Kontrollera om vi nått max
                    if (nuvarandeAntalClowner >= maxAntalClowner)
                    {
                        Debug.Log("Spelet är slut 10 clowner har landat i poolen.");
                        Time.timeScale = 0f; // Stoppa spelet
                    }
                }
            }
        }
    }
}