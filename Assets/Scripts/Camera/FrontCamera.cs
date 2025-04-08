using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontCamera : MonoBehaviour
{
    public RawImage cameraPreview;   // The RawImage that shows the live preview
    public Image capturedImageDisplay; // Image to display captured photo
    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // Find front-facing camera
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                webcamTexture = new WebCamTexture(devices[i].name);
                break;
            }
        }

        if (webcamTexture != null)
        {
            cameraPreview.texture = webcamTexture;
            cameraPreview.material.mainTexture = webcamTexture;
            webcamTexture.Play();

            // Adjust the camera feed aspect ratio to fit inside the RawImage using AspectRatioFitter
            AdjustAspectRatio();
        }
        else
        {
            Debug.LogWarning("No front-facing camera found.");
        }
    }

    private void AdjustAspectRatio()
    {
        // Get the aspect ratio of the webcam feed
        float aspectRatio = (float)webcamTexture.width / (float)webcamTexture.height;

        // Get the AspectRatioFitter component from the cameraPreview RawImage
        AspectRatioFitter fitter = cameraPreview.GetComponent<AspectRatioFitter>();

        // Set the aspect ratio of the webcam feed
        fitter.aspectRatio = aspectRatio;
    }
    public void CapturePhotoWithFixedSize()
    {
        CaptureCroppedPhoto(512, 512); 
    }
    public void CaptureCroppedPhoto(int targetWidth, int targetHeight)
    {
        // Step 1: Get raw camera image
        Texture2D rawTex = new Texture2D(webcamTexture.width, webcamTexture.height);
        rawTex.SetPixels(webcamTexture.GetPixels());
        rawTex.Apply();

        // Step 2: Crop to center using target aspect ratio
        Texture2D cropped = CropCenter(rawTex, targetWidth, targetHeight);

        // Step 3: Convert to sprite
        Sprite photoSprite = Sprite.Create(
            cropped,
            new Rect(0, 0, cropped.width, cropped.height),
            new Vector2(0.5f, 0.5f)
        );

        capturedImageDisplay.sprite = photoSprite;
    }

    private Texture2D CropCenter(Texture2D source, int targetWidth, int targetHeight)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;

        float targetAspect = (float)targetWidth / targetHeight;
        int cropWidth = sourceWidth;
        int cropHeight = Mathf.RoundToInt(sourceWidth / targetAspect);

        if (cropHeight > sourceHeight)
        {
            cropHeight = sourceHeight;
            cropWidth = Mathf.RoundToInt(sourceHeight * targetAspect);
        }

        int x = (sourceWidth - cropWidth) / 2;
        int y = (sourceHeight - cropHeight) / 2;

        Color[] pixels = source.GetPixels(x, y, cropWidth, cropHeight);

        Texture2D croppedTex = new Texture2D(cropWidth, cropHeight);
        croppedTex.SetPixels(pixels);
        croppedTex.Apply();

        return croppedTex;
    }
}
