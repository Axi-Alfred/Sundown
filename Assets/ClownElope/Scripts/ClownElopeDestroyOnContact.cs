using UnityEngine;

public class ClownElopeDestroyOnContact : MonoBehaviour
{
    // Referens till spelets huvudmanager
    private ClownElopeManager gameManager;

    // Start kallas en gång i början
    void Start()
    {
        // Hitta manager-objektet i scenen
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

    // När ett objekt träffar denna kolliderare
    void OnTriggerEnter2D(Collider2D other)
    {
        // Skriv ut meddelande i konsollen
        Debug.Log("Objektet flydde och träffade väggen!");
        
        // Meddela game manager att ett objekt flydde
        gameManager.ObjectEscaped();
        
        // Förstör objektet som träffade
        Destroy(other.gameObject);
    }
}