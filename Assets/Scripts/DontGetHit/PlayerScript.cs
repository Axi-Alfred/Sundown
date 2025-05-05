using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Rörelseinställningar")]
    public float moveSpeed = 15f;
    public float xBoundary = 17.8f;

    [Header("Optimering")]
    public bool showDebugGizmos = true;
    public float inputSmoothing = 0.2f;

    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 touchStartPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Update()
    {
        HanteraTouchInput();
    }

    void FixedUpdate()
    {
        if (!isDragging)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        BegränsaPosition();
    }

    // 🔹 Touchstyrning för mobiltelefon
    void HanteraTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = worldTouchPos;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        // Mjuk övergång (smoothing)
                        if (inputSmoothing > 0)
                        {
                            worldTouchPos = Vector2.Lerp(touchStartPosition, worldTouchPos, 1f + inputSmoothing);
                        }

                        float dragDistance = worldTouchPos.x - touchStartPosition.x;
                        rb.velocity = new Vector2(Mathf.Clamp(dragDistance * 8f, -moveSpeed, moveSpeed), 0);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    rb.velocity = Vector2.zero;
                    break;
            }
        }
        else
        {
            isDragging = false;
        }
    }

    // 🔹 Hindra spelaren från att åka utanför skärmen
    void BegränsaPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xBoundary, xBoundary);
        transform.position = pos;
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        Rigidbody2D currentRb = rb != null ? rb : GetComponent<Rigidbody2D>();
        if (currentRb == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentRb.velocity);
    }
}