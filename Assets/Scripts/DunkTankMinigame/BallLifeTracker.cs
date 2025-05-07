using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLifeTracker : MonoBehaviour
{
    public SwipeThrow throwController; // Reference to the SwipeThrow script so we can notify it when the ball is gone
    public GameObject poofPrefab; // Optional particle effect prefab

    // Called automatically by Unity when this object goes off-screen 
    void OnBecameInvisible()
    {
        SpawnPoof(); // Spawn particle effect at ball's position 
        throwController?.BallDestroyed(); // Notify SwipeThrow that the ball is gone
        Destroy(gameObject); // Destroy this ball object to free memory
    }

    // Called automatically when the ball collides with another 2D object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ball hits something tagged "Target"
        if (collision.gameObject.CompareTag("Target"))
        {
            throwController?.BallDestroyed(); // Notify SwipeThrow to reset the ball state
            Destroy(collision.gameObject); // optional: destroy target
            SpawnPoof(); // Spawn particle effect
            Destroy(gameObject); // Destroy the ball itself after the collision
        }
    }

    // Helper method to spawn the particle effect
    void SpawnPoof()
    {
        if (poofPrefab != null)
        {
            // Create a new instance of the poof effect at the ball's current position
            Instantiate(poofPrefab, transform.position, Quaternion.identity);
        }
    }
}
