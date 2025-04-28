using UnityEngine;

public class AlignSpotsRuntime : MonoBehaviour
{
    public RectTransform[] handButtons; // Your 4 hand UI buttons (RectTransform)
    public Transform[] spots;           // Your 4 world-space spot objects
    public Camera uiCamera;              // Your Main Camera

    void Update()
    {
        for (int i = 0; i < handButtons.Length; i++)
        {
            // Step 1: Convert the UI Button world position to Screen position
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, handButtons[i].position);

            // Step 2: Use ScreenToWorldPoint correctly (with proper Z depth)
            float distanceToCamera = Mathf.Abs(uiCamera.transform.position.z);
            Vector3 screenPosition = new Vector3(screenPoint.x, screenPoint.y, distanceToCamera);
            Vector3 worldPos = uiCamera.ScreenToWorldPoint(screenPosition);

            // Step 3: Flatten to 2D
            worldPos.z = 0f;

            // Step 4: Move the Spot
            spots[i].position = worldPos;
        }
    }
}
