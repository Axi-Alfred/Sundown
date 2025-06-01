using UnityEngine;

// Skript som triggar tältanimationen när en clown når mitten
public class ClownElopeTentAnimationTrigger : MonoBehaviour
{
    // Referens till tält Animator
    public Animator tentAnimator;

    // Referens till spelmanagern
    private ClownElopeManager gameManager;

    // Start körs en gång när spelet startar
    void Start()
    {
        // Om tentAnimator inte är tilldelad, hämta den från samma GameObject
        if (tentAnimator == null)
        {
            tentAnimator = GetComponent<Animator>();
        }

        // Kontrollera om Animator finns
        if (tentAnimator == null)
        {
            Debug.LogError("Tent Animator är inte tilldelad! Lägg till en Animator på detta GameObject.");
        }

        // Hämta referensen till ClownElopeManager i scenen
        gameManager = FindObjectOfType<ClownElopeManager>();
        if (gameManager == null)
        {
            Debug.LogError("ClownElopeManager hittades inte! Lägg till en i scenen.");
        }
    }

    // OnTriggerEnter2D körs när en collider går in i triggers kollidern
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kontrollera om objektet har taggen "Draggable"
        if (other.CompareTag("Draggable"))
        {
            // Spela tältanimationen
            if (tentAnimator != null)
            {
                tentAnimator.SetTrigger("Celebrate");
            }
            else
            {
                Debug.LogWarning("Animator saknas, kan inte spela Celebrate-animationen!");
            }

            // Meddela att clownen har räddats
            if (gameManager != null)
            {
                gameManager.ObjectSaved();
            }

            // Förstör clownobjektet som nådde mitten
            Destroy(other.gameObject);
        }
    }
}