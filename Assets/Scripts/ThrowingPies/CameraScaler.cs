using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // Use 16:9 as default reference

    void Start()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            Rect rect = camera.rect;

            float scaleWidth = 1.0f / scaleHeight;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}

