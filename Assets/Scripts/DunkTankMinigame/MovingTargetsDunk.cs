using UnityEngine;

public class MovingTargetsDunk : MonoBehaviour
{
    public float speed = 2f;
    public float range = 2f;

    private Vector3 startPos;
    private bool hasBeenHit = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (hasBeenHit) return;

        float offset = Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenHit) return;

        if (other.CompareTag("PlayerProjectile")) // or whatever hits the target
        {
            hasBeenHit = true;

            // Register the hit
            if (TargetCounterGameManager.Instance != null)
                TargetCounterGameManager.Instance.RegisterHit();

            // Optional: visual feedback or destruction
            Destroy(gameObject);
        }
    }
}
