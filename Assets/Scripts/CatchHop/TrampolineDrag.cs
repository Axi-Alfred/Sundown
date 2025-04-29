using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineDrag : MonoBehaviour
{
    private Vector3 touchOffset;

    void Update()
    {
        // Kolla om vi är på mobil (touch)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.position);
        }
        // Kolla om vi är i Unity Editor (mus)
        else if (Application.isEditor && Input.GetMouseButton(0))
        {
            HandleTouch(Input.mousePosition);
        }
    }

    void HandleTouch(Vector2 screenPosition)
    {
        // Omvandla skärmens koordinater till världens position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0;

        // Flytta trampolinen till den nya positionen (endast X-led)
        transform.position = new Vector3(worldPos.x, transform.position.y, 0);
    }
}
