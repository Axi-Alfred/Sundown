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
    private bool isDragging = false; // Saknad deklaration
    private Vector2 touchStartPosition; // Saknad deklaration

    void Awake()
    {
        // Hämta Rigidbody2D tidigare i livscykeln
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Dubbla säkerhet - se till att rb är tilldelad
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Update()
    {
        HanteraInput();
    }

    private void FixedUpdate()
    {
        // Omedelbart stopp när ingen input registreras
        if (!isDragging)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        BegränsaPosition();
    }

    void HanteraInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Applicera input-jämning om aktiverad
            if (inputSmoothing > 0)
            {
                currentPos = Vector2.Lerp(touchStartPosition, currentPos, 1f + inputSmoothing);
            }

            float dragDistance = currentPos.x - touchStartPosition.x;
            // Direkt hastighetskontroll baserad på fingerposition
            rb.velocity = new Vector2(Mathf.Clamp(dragDistance * 8f, -moveSpeed, moveSpeed), 0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb.velocity = Vector2.zero; // Omedelbart stopp
        }
    }

    void BegränsaPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xBoundary, xBoundary);
        transform.position = pos;
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        // Säkerställ att rb finns innan vi använder den
        Rigidbody2D currentRb = rb != null ? rb : GetComponent<Rigidbody2D>();
        if (currentRb == null) return;

        // Visa hastighet
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentRb.velocity);

        // Visa dragriktning
        if (isDragging && Input.mousePresent) // Extra säkerhetskontroll
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(touchStartPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}