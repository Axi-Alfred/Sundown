using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{
    private float deadZone = -11.88f;
    private bool isDeadly = true;

    void Update()
    {
        // Hämta nuvarande hastighet
        float currentSpeed = DifficultyManagerScript.Instance.GetCurrentSpeed();

        // Flytta blocket nedåt
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - currentSpeed * Time.deltaTime,
            transform.position.z
        );

        // Förstör block under deadZone
        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isDeadly = false;
            Destroy(gameObject);
        }
    }

    // Returnerar om blocket är farligt
    public bool IsDeadly()
    {
        return isDeadly;
    }
}