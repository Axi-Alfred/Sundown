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
    private bool isAlive = true; // Håller koll på om spelaren lever

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
        if (isAlive) // Endast hantera input om spelaren lever
        {
            HanteraTouchInput();
        }
    }

    void FixedUpdate()
    {
        if (!isDragging || !isAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        BegränsaPosition();
    }

    // Hanterar touch-input för mobil
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

    // Förhindrar att spelaren rör sig utanför skärmen
    void BegränsaPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xBoundary, xBoundary);
        transform.position = pos;
    }

    // Kollisionshantering för knivar/block
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            BlockMoveScript block = collision.GetComponent<BlockMoveScript>();
            if (block != null && block.IsDeadly())
            {
                Dö();
            }
        }
    }

    // Hanterar spelarens död
    void Dö()
    {
        if (!isAlive) return; // Redan död
        
        isAlive = false;
        Debug.Log("Spelaren dog!");
        // Här kan du lägga till fler effekter som:
        // - Spela ljudeffekt
        // - Visa game over-skärm
        // - Vibrera telefonen
        Time.timeScale = 0; // Pausa spelet
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