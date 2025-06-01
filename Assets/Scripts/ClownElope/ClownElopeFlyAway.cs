using UnityEngine;

public class ClownElopeFlyAway : MonoBehaviour
{
    public float speed = 2f; // Fart för clownens rörelse

    void Start()
    {
        // Välj en slumpmässig riktning (360° cirkel)
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Sätt clownens fart
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = randomDirection * speed;
        }
    }
}