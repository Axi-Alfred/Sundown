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
    private bool isAlive = true;

    // Start kallas före första uppdateringen
    void Start()
    {
        // Hämta Rigidbody2D-komponenten
        rb = GetComponent<Rigidbody2D>();

        // Dubbelkolla om vi har Rigidbody2D
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Fryser spelarens rotation och Y-position
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Uppdateras varje frame
    void Update()
    {
        // Hantera input endast om spelaren lever
        if (isAlive == true)
        {
            HanteraTouchInput();
        }
    }

    // Fast uppdatering för fysikberäkningar
    void FixedUpdate()
    {
        // Stanna direkt om vi inte drar eller är döda
        if (isDragging == false || isAlive == false)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        BegränsaPosition();
    }

    // Hanterar touch-input för mobil
    void HanteraTouchInput()
    {
        // Kolla om det finns touch-input
        if (Input.touchCount > 0)
        {
            // Ta första touch-inputet
            Touch touch = Input.GetTouch(0);

            // Områdeskontroll för touch
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = Camera.main.ScreenToWorldPoint(touch.position);
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isDragging == true)
                {
                    Vector2 currentPos = Camera.main.ScreenToWorldPoint(touch.position);

                    // Applicera input-jämning
                    if (inputSmoothing > 0f)
                    {
                        currentPos.x = Mathf.Lerp(touchStartPosition.x, currentPos.x, 1f + inputSmoothing);
                        currentPos.y = Mathf.Lerp(touchStartPosition.y, currentPos.y, 1f + inputSmoothing);
                    }

                    // Beräkna dragavstånd
                    float dragDistance = currentPos.x - touchStartPosition.x;

                    // Beräkna hastighet
                    float calculatedSpeed = dragDistance * 8f;

                    // Begränsa maxhastighet
                    if (calculatedSpeed > moveSpeed)
                    {
                        calculatedSpeed = moveSpeed;
                    }
                    else if (calculatedSpeed < -moveSpeed)
                    {
                        calculatedSpeed = -moveSpeed;
                    }

                    // Sätt hastigheten
                    rb.velocity = new Vector2(calculatedSpeed, 0f);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            isDragging = false;
        }
    }

    // Ser till att spelaren inte åker utanför skärmen
    void BegränsaPosition()
    {
        Vector3 newPosition = transform.position;

        // Kolla vänster gräns
        if (newPosition.x < -xBoundary)
        {
            newPosition.x = -xBoundary;
        }

        // Kolla höger gräns
        if (newPosition.x > xBoundary)
        {
            newPosition.x = xBoundary;
        }

        transform.position = newPosition;
    }

    // Hanterar kollision med block
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kolla om det är ett block
        if (other.tag == "Block")
        {
            BlockMoveScript blockScript = other.GetComponent<BlockMoveScript>();

            if (blockScript != null)
            {
                // Kolla om blocket är farligt
                bool ärBlocketFarligt = blockScript.IsDeadly();

                if (ärBlocketFarligt == true)
                {
                    SpelareDöd();
                }
            }
        }
    }

    // Hanterar spelarens död
    void SpelareDöd()
    {
        isAlive = false;
        Debug.Log("Spelaren dog!");
        GameManager1.EndTurn();
    }

    // Rita debug-information
    void OnDrawGizmos()
    {
        if (showDebugGizmos == true)
        {
            Rigidbody2D currentRb = rb;

            // Säkerhetskontroll
            if (currentRb == null)
            {
                currentRb = GetComponent<Rigidbody2D>();
            }

            if (currentRb != null)
            {
                Gizmos.color = Color.red;
                Vector3 velocityLine = new Vector3(currentRb.velocity.x, currentRb.velocity.y, 0f);
                Gizmos.DrawLine(transform.position, transform.position + velocityLine);
            }
        }
    }
}