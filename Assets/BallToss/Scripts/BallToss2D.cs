using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToss2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isDragging = false;
    private Vector3 originalScale;
    private float screenHeightThreshold = 0.25f; // Endast tillåt drag från nedre delen av skärmen
    public float maxDragY = 1f; // Hur högt upp spelaren får dra bollen

    public float tossForce = 5f;        // Hur hårt bollen kastas
    public float shrinkSpeed = 0.2f;    // Hur snabbt bollen krymper

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        rb.gravityScale = 0f; // Ingen gravitation i början
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            worldPos.z = 0; // Vi jobbar i 2D

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("TOUCH BEGAN");
                    startTouchPos = touch.position;

                    // Kontrollera om touchen är inom den tillåtna delen av skärmen
                    if (touch.position.y < Screen.height * screenHeightThreshold)
                    {
                        isDragging = true;
                        rb.velocity = Vector2.zero; // Stoppa eventuell tidigare rörelse
                        rb.gravityScale = 0f; // Stäng av gravitation medan man drar
                    }
                    else
                    {
                        Debug.Log("Touchen var utanför tillåtet område");
                        isDragging = false;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        // Begränsa hur högt upp spelaren får dra bollen
                        float clampedY = Mathf.Min(worldPos.y, maxDragY);
                        transform.position = new Vector3(worldPos.x, clampedY, 0);
                    }
                    break;

                case TouchPhase.Ended:
                    Debug.Log("TOUCH ENDED");
                    endTouchPos = touch.position;
                    TossBall(startTouchPos, endTouchPos);
                    isDragging = false;
                    break;
            }
        }
    }

    // Funktion för att kasta bollen baserat på dragrörelse
    void TossBall(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;

        if (direction.magnitude > 100f)
        {
            rb.gravityScale = 1f; // Aktivera gravitation igen

            // "Kasta" bollen uppåt på skärmen, vilket i spelet är nedåt i världen
            Vector2 tossDirection = new Vector2(0, -1).normalized;
            rb.AddForce(tossDirection * tossForce, ForceMode2D.Impulse);

            Debug.Log("Bollen kastades!");
        }
    }
}