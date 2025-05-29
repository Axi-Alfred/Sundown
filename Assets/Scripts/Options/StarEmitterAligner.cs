using UnityEngine;

public class StarEmitterAligner : MonoBehaviour
{
    public ParticleSystem top, bottom, left, right;
    public Camera mainCamera;

    void Awake()
    {
        // Auto-assign if not set
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Start()
    {
        float zOffset = 1f;

        Vector3 topPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, mainCamera.nearClipPlane + zOffset));
        Vector3 bottomPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, mainCamera.nearClipPlane + zOffset));
        Vector3 leftPos = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, mainCamera.nearClipPlane + zOffset));
        Vector3 rightPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, mainCamera.nearClipPlane + zOffset));

        top.transform.position = topPos;
        bottom.transform.position = bottomPos;
        left.transform.position = leftPos;
        right.transform.position = rightPos;
    }
}
