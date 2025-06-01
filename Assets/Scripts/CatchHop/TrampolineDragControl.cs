using UnityEngine;

public class TrampolineDragControl : MonoBehaviour
{
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    private float minX;
    private float maxX;

    void Start()
    {
        // Sätt upp gränser för hur långt trampolinen får röra sig
        SetUpMovementBoundaries();
    }

    void Update()
    {
        // Hantera touch (mobil)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Spara var vi började trycka
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // Flytta trampolinen när vi drar fingret
                MoveWithFinger(touch.position);
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Sluta dra
                isDragging = false;
            }
        }
        // Hantera mus (för test i Unity Editor)
        else if (Application.isEditor && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            if (!isDragging)
            {
                lastTouchPosition = mousePosition;
                isDragging = true;
            }

            MoveWithFinger(mousePosition);
            lastTouchPosition = mousePosition;
        }
        else
        {
            // Ingen input – dra inte
            isDragging = false;
        }
    }

    void MoveWithFinger(Vector2 newPosition)
    {
        // Räkna ut skillnaden i skärmposition
        Vector2 delta = newPosition - lastTouchPosition;

        // Gör om skillnaden till spelvärlden
        Vector3 worldDelta = Camera.main.ScreenToWorldPoint(new Vector3(newPosition.x, newPosition.y, 0)) -
                             Camera.main.ScreenToWorldPoint(new Vector3(lastTouchPosition.x, lastTouchPosition.y, 0));

        // Hämta nuvarande position
        Vector3 currentPos = transform.parent.position;

        // Lägg till rörelse i X-led
        float newX = currentPos.x + worldDelta.x;

        // Begränsa rörelsen så trampolinen stannar inom skärmen
        newX = Mathf.Clamp(newX, minX, maxX);

        // Flytta trampolinen till nya positionen
        transform.parent.position = new Vector3(newX, currentPos.y, 0);
    }

    void SetUpMovementBoundaries()
    {
        // Hämta SpriteRenderer för att räkna ut objektets bredd
        SpriteRenderer spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();

        float halfWidth = 0.5f;

        if (spriteRenderer != null)
        {
            // Räkna ut halva bredden från sprite:ns bounds
            halfWidth = spriteRenderer.bounds.extents.x;
        }

        // Hämta skärmens vänster- och högerkant i spelvärlden
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // Sätt gränserna baserat på halva objektets bredd
        minX = leftEdge.x + halfWidth;
        maxX = rightEdge.x - halfWidth;
    }

}