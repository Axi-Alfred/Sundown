using UnityEngine;

public class PoolBottomBlocker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D annanCollider)
    {
        // Kontrollera om det är en clown som försöker komma in
        if (annanCollider.CompareTag("Clown"))
        {
            // Ta bort clownen direkt för att stoppa den från att komma in underifrån
            Destroy(annanCollider.gameObject);
        }
    }
}