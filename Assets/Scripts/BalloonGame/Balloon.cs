using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float wobbleIntensity = 2f;
    public float rotationSpeed = 100f;

    public bool isNegative = false;

    private Vector2 direction;
    private float wobbleOffset;

    void Start()
    {
        float angle = Random.Range(0f, 360f); // Full chaos
        float radians = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        wobbleOffset = Random.Range(0f, Mathf.PI * 2f);
        rotationSpeed = Random.Range(-180f, 180f);
    }

    void Update()
    {
        float wobble = Mathf.Sin(Time.time * 5f + wobbleOffset) * wobbleIntensity;
        Vector3 movement = (direction + new Vector2(wobble, 0)) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Destroy if off-screen
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= -0.1f && screenPoint.x <= 1.1f && screenPoint.y >= -0.1f && screenPoint.y <= 1.1f;
    }
}
