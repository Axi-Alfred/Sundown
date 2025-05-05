using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{
    private float deadZone = -11.88f;
    private bool isDeadly = true; // Avgör om blocket kan skada spelaren

    void Update()
    {
        float currentSpeed = DifficultyManagerScript.Instance.GetCurrentSpeed();
        transform.position += Vector3.down * currentSpeed * Time.deltaTime;

        // Förstör blocket om det hamnar under skärmen
        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isDeadly = false; // Blocket kan inte längre skada spelaren
            Destroy(gameObject); // Förstör blocket när det träffar marken
        }
    }

    // Returnerar om blocket är farligt (kan skada spelaren)
    public bool IsDeadly()
    {
        return isDeadly;
    }
}