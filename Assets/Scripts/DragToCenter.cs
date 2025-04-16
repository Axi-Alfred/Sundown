using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToCenter : MonoBehaviour
{
    public bool isDragging = false;  // public, so other scripts can see it
    private Vector3 offset;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - touchPos;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                transform.position = touchPos + offset;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}
