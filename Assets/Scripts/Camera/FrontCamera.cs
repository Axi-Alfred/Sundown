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

    private float aspectRatio;
    private void OnEnable()
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
        webcamTexture.Play();
        aspectRatio = (float)webcamTexture.width / (float)webcamTexture.height;

        AdjustRawImageAspectRation();
    }

    private void AdjustRawImageAspectRation()
    {
        RectTransform rectTransform = cameraPreview.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspectRatio);
        rectTransform.localScale = new Vector3(-1, 1, 1);
    }

    private void AdjustTakenImageAspectRatio()
    {
        RectTransform rectTransform = capturedImageDisplay.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspectRatio);
        rectTransform.localScale = new Vector3(-1, 1, 1);
    }

    private IEnumerator TakePicture()
    {
        capturedImageDisplay.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();

        Color[] pixels = webcamTexture.GetPixels();
        Texture2D picTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
        picTexture.SetPixels(pixels);
        Sprite picSprite = Sprite.Create(picTexture, new Rect(0, 0, picTexture.width, picTexture.height), new Vector2(0.5f, 0.5f));
        capturedImageDisplay.sprite = picSprite;
        picTexture.Apply();

        AdjustTakenImageAspectRatio();

        cameraPreview.gameObject.SetActive(false);
        takeButton.SetActive(false);
        retakeButton.SetActive(true);
    }

    public void TakePictureButton()
    {
        StartCoroutine(TakePicture());
    }

    public void RetakePictureButton()
    {
        cameraPreview.gameObject.SetActive(true);

        capturedImageDisplay.sprite = null;
        capturedImageDisplay.gameObject.SetActive(false);
        retakeButton.SetActive(false);
        takeButton.SetActive(true);
    }
}
