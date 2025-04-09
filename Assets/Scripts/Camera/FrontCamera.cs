using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontCamera : MonoBehaviour
{
    [SerializeField] private RawImage cameraPreview;   
    [SerializeField] private Image capturedImageDisplay;
    [SerializeField] private GameObject takeButton;
    [SerializeField] private GameObject retakeButton;

    private WebCamTexture webcamTexture;
    private WebCamDevice webcamDevice;

    void Start()
    {
        foreach (WebCamDevice i in WebCamTexture.devices)
        {
            if (i.isFrontFacing)
            {
                webcamDevice = i;

                break;
            }
        }

        webcamTexture = new WebCamTexture(webcamDevice.name);

        cameraPreview.texture = webcamTexture;
        cameraPreview.rectTransform.localScale = new Vector3(-1, 1, 1);
        webcamTexture.Play();
       
    }   
}
