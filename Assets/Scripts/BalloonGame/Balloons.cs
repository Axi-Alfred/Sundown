using UnityEngine;

public class Balloons : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool isBlack = false;

    private Vector2 driftDirection;
    private float driftSpeed;
    private float bobFrequency;
    private float bobAmplitude;
    private float wiggleOffset;

    void Start()
    {
        // Slight random drift direction
        driftDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;

        // BOOSTED speed
        driftSpeed = Random.Range(3f, 6f); // More aggressive balloon speed

        // Wobble animation
        bobFrequency = Random.Range(1.5f, 3f);  // Faster bobbing
        bobAmplitude = Random.Range(0.2f, 0.4f); // Slightly more wiggly
        wiggleOffset = Random.Range(0f, Mathf.PI * 2f);
    }


    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobFrequency + wiggleOffset) * bobAmplitude;
        Vector3 movement = new Vector3(driftDirection.x + bob, driftDirection.y, 0f) * driftSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);

        transform.Rotate(0f, 0f, Mathf.Sin(Time.time + wiggleOffset) * 10f * Time.deltaTime); // Swaying rotation

        if (transform.position.y > 6f)
            Destroy(gameObject);
    }



    void OnMouseDown()
    {
        if (isBlack)
        {
            BalloonGameManager.Instance.TriggerBlackBalloon();
        }
        else
        {
            BalloonGameManager.Instance.RegisterBalloonPop();
            Destroy(gameObject);
        }

    }
}
