using UnityEngine;

public class TrampolineDragControl : MonoBehaviour
{
    void Update()
    {
        // Hantera touch-input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleInput(touch.position);
        }
        // Hantera mus-input i editorn
        else if (Application.isEditor && Input.GetMouseButton(0))
        {
            HandleInput(Input.mousePosition);
        }
    }

    void HandleInput(Vector2 screenPosition)
    {
        // Omvandla till spelvärldens position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;

        // För att flytta hela TrampolineUnit, inte bara clownerna
        transform.parent.position = new Vector3(worldPosition.x, transform.parent.position.y, 0);

    }
}