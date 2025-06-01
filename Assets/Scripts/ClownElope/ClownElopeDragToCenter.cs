using UnityEngine;

// Skript som hanterar att dra objektet till mitten
public class ClownElopeDragToCenter : MonoBehaviour
{
    // Flagga för att se om objektet dras just nu (synlig för andra skript)
    public bool isDragging = false;

    // Förskjutning mellan pekpunkt och objektets centrum
    private Vector3 offset;

    // Referens till spelmanagern
    private ClownElopeManager gameManager;

    // Start körs innan första uppdateringen
    void Start()
    {
        // Hämta referensen till ClownElopeManager i scenen
        gameManager = FindObjectOfType<ClownElopeManager>();

        // Kontrollera om spelmanagern finns
        if (gameManager == null)
        {
            Debug.LogError("ClownElopeManager hittades inte! Se till att det finns en i scenen.");
        }
    }

    // Update kallas en gång per frame
    void Update()
    {
        // Kontrollera om spelmanagern är tilldelad och om spelet är över
        if (gameManager != null && gameManager.IsGameOver())
        {
            return; // Avsluta om spelet är över
        }

        // Kontrollera om det finns pekningar (touch) på skärmen
        if (Input.touchCount > 0)
        {
            // Hämta den första pekningen
            Touch touch = Input.GetTouch(0);

            // Omvandla skärmposition till världsposition
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f; // Nollställ Z-axeln för 2D

            // När en ny pekning startar
            if (touch.phase == TouchPhase.Began)
            {
                // Kolla om vi trycker på rätt objekt
                Collider2D hit = Physics2D.OverlapPoint(touchPos);
                if (hit != null && hit.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - touchPos;
                }
            }
            // När pekningen rör sig över skärmen
            else if (touch.phase == TouchPhase.Moved && isDragging == true)
            {
                // Uppdatera objektets position med förskjutningen
                transform.position = touchPos + offset;
            }
            // När pekningen släpps
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false; // Sluta dra
            }
        }
    }
}