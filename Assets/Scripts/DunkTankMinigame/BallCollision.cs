using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    // Called automatically when this GameObject collides with another 2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the target the ball hit has the tag "Target"
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit target!");
            Destroy(collision.gameObject); // Destroy the target upon collision with the ball
            Destroy(gameObject); // Destroy the ball that hit the target
        }
    }

    // Called automatically by Unity when the target is no longer visible by any camera
    private void OnBecameInvisible()
    {
        // Destroy the ball to avoid leaving it in the scene
        Destroy(gameObject);
    }
}
