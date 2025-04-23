using UnityEngine;

public class VikingFaller : MonoBehaviour
{
    public float rotationSpeed = 180f; // Degrees per second

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Destroy if it falls below the screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
