using UnityEngine;

public class Viking : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Helmet"))
        {
            Debug.Log("Caught!");
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Missed
    }
}
