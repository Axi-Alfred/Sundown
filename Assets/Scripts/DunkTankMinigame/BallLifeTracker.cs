using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLifeTracker : MonoBehaviour
{
    public SwipeThrow throwController;
    public GameObject poofPrefab; // Assign in code or in prefab

    void OnBecameInvisible()
    {
        SpawnPoof();
        throwController?.BallDestroyed();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            throwController?.BallDestroyed();
            Destroy(collision.gameObject); // optional: destroy target
            SpawnPoof();
            Destroy(gameObject);
        }
    }

    void SpawnPoof()
    {
        if (poofPrefab != null)
        {
            Instantiate(poofPrefab, transform.position, Quaternion.identity);
        }
    }
}
