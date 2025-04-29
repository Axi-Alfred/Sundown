using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
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
