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
        driftDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        driftSpeed = Random.Range(3f, 6f);
        bobFrequency = Random.Range(1.5f, 3f);
        bobAmplitude = Random.Range(0.2f, 0.4f);
        wiggleOffset = Random.Range(0f, Mathf.PI * 2f);

        // Force Z to 0 to stay visible
        Vector3 fixZ = transform.position;
        fixZ.z = 0f;
        transform.position = fixZ;
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobFrequency + wiggleOffset) * bobAmplitude;
        Vector3 movement = new Vector3(driftDirection.x + bob, driftDirection.y, 0f) * driftSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        transform.Rotate(0f, 0f, Mathf.Sin(Time.time + wiggleOffset) * 10f * Time.deltaTime);

        if (transform.position.y > 6f)
            Destroy(gameObject);

#if UNITY_ANDROID || UNITY_IOS
        CheckTouch();
#else
        CheckClick(); // Still works in editor
#endif
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero);
            if (hit && hit.collider.gameObject == gameObject)
                Pop();
        }
    }

    void CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero);
                if (hit && hit.collider.gameObject == gameObject)
                    Pop();
            }
        }
    }

    void Pop()
    {
        if (isBlack)
        {
            BalloonGameManager.Instance.TriggerBlackBalloon();
            SFX.Play(2);
        }
        else
        {
            BalloonGameManager.Instance.RegisterBalloonPop();
            SFX.Play(1); // Your pop sound
        }

        Destroy(gameObject);
    }
}
