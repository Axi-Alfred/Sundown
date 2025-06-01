using UnityEngine;

public class CatchClown_PoolCounter : MonoBehaviour
{
    public int maxAntalClowner = 10; // Max antal clowner som får landa i poolen
    private int nuvarandeAntalClowner = 0; // Räknare för landade clowner

    void OnTriggerEnter2D(Collider2D annanCollider)
    {
        Debug.Log("Something entered the pool trigger!");
        Debug.Log("Entered object: " + annanCollider.gameObject.name + ", Tag: " + annanCollider.tag);

        if (annanCollider.CompareTag("Clown"))
        {
            Rigidbody2D rb = annanCollider.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.y <= 0)
            {
                // Ta bort clownen direkt
                Destroy(annanCollider.gameObject);

                // Öka räknaren
                nuvarandeAntalClowner = nuvarandeAntalClowner + 1;

                // Skriv ut debug-meddelande
                Debug.Log("Clown count: " + nuvarandeAntalClowner);

                // Kontrollera om vi nått max
                if (nuvarandeAntalClowner >= maxAntalClowner)
                {
                    FindObjectOfType<StarBurstDOTween>().TriggerBurst();
                    PlayerData.currentPlayerTurn.AddScore(1);
                    GameManager1.EndTurn();
                    Debug.Log("Spelet är slut: 10 clowner har landat i poolen.");
                }
            }
        }
    }
}