using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float wobbleIntensity = 2f;
    public float rotationSpeed = 100f;

    public bool isNegative = false;

    private float wobbleOffset;

    void Start()
    {
        wobbleOffset = Random.Range(0f, Mathf.PI * 2f);
        rotationSpeed = Random.Range(-180f, 180f);
    }

    void Update()
    {
        // Upward floating with wobble
        float wobble = Mathf.Sin(Time.time * 5f + wobbleOffset) * wobbleIntensity;
        Vector3 movement = new Vector3(wobble, 1f, 0f).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif

        if (!IsVisible())
            Destroy(gameObject);
    }

    void OnMouseDown()
    {
        Pop(); // Works in editor and standalone
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

        if (hit != null && hit.gameObject == gameObject)
        {
            Pop();
        }
    }

    void Pop()
    {
        Debug.Log(isNegative ? "💀 Negative balloon popped!" : "🎈 Balloon popped!");
        BalloonGameManager.Instance.BalloonPopped(isNegative);
        Destroy(gameObject);
    }

    bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= -0.1f && screenPoint.x <= 1.1f && screenPoint.y >= -0.1f && screenPoint.y <= 1.1f;
    }
}
