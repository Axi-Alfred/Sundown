using UnityEngine;

public class TrampolineDrag : MonoBehaviour
{
    void Update()
    {
        // Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleInput(touch.position);
        }
        // Mouse input (editor or desktop)
        else if (Application.isEditor && Input.GetMouseButton(0))
        {
            HandleInput(Input.mousePosition);
        }
    }

    void HandleInput(Vector2 screenPosition)
    {
        // Convert screen position to world coordinates
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0;

        // Only move in X-axis
        transform.position = new Vector3(worldPos.x, transform.position.y, 0);
    }
}
