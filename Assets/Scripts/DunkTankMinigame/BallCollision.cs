using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit target!");
            Destroy(collision.gameObject); // or play animation
            Destroy(gameObject); // destroy ball too if you want
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
