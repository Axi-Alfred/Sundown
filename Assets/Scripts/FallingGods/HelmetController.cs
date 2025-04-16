using UnityEngine;

public class HelmetController : MonoBehaviour
{
    private Vector2 touchStartPos;
    private Vector3 helmetStartPos;
    private bool dragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    dragging = true;
                    helmetStartPos = transform.position;
                    touchStartPos = touchPos;
                    break;

                case TouchPhase.Moved:
                    if (dragging)
                    {
                        float deltaX = touchPos.x - touchStartPos.x;
                        transform.position = new Vector3(helmetStartPos.x + deltaX, helmetStartPos.y, 0);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    dragging = false;
                    break;
            }
        }
    }
}
