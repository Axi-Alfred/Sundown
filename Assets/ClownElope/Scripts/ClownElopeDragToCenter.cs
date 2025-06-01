using UnityEngine;

// Skript som hanterar att dra objektet till mitten
public class ClownElopeDragToCenter : MonoBehaviour
{
    // Flagga för att se om objektet dras just nu (synlig för andra skript)
    public bool isDragging = false;

    // Förskjutning mellan pekpunkt och objektets centrum
    private Vector3 offset;

    // Update kallas en gång per frame
    void Update()
    {
        // Kontrollera om det finns pekningar (touch) på skärmen
        if (Input.touchCount > 0)
        {
            // Hämta den första pekningen
            Touch touch = Input.GetTouch(0);
            // Omvandla skärmposition till världsposition
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f; // Nollställ Z-axeln för 2D

            // Börjar ny pekning
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D hit = Physics2D.OverlapPoint(touchPos);
                if (hit != null && hit.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - touchPos;
                }
            }
            // Under pågående drag när fingret rör sig
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // Uppdatera objektets position med förskjutningen
                transform.position = touchPos + offset;
            }
            // När pekningen avslutas (lyfter fingret)
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false; // Sluta dra
            }
        }
    }
}