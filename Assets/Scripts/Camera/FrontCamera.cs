using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class FrontCamera : MonoBehaviour
{
    [SerializeField] private RawImage cameraPreview;   
    [SerializeField] private Image capturedImageDisplay;
    [SerializeField] private GameObject takeButton;
    [SerializeField] private GameObject retakeButton;
    [SerializeField] private GameObject cameraSystemObject;
    [SerializeField] private GameObject playerSelectionObject;

    private WebCamTexture webcamTexture;
    private WebCamDevice webcamDevice;

    private float aspectRatio;
    private float imageRotation = -90;
    //Player specifics

    private Player currentPlayer;

    private void Start()
    {
        cameraSystemObject.gameObject.SetActive(false);
        playerSelectionObject.gameObject.SetActive(true);

        StartCoroutine(WaitForCameraPermission());
    }
    private void InitializeCamera()
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
        rectTransform.localRotation = Quaternion.Euler(0, 0, imageRotation);
    }

    private void AdjustTakenImageAspectRatio()
    {
        RectTransform rectTransform = capturedImageDisplay.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspectRatio);
        rectTransform.localScale = new Vector3(-1, 1, 1);
        rectTransform.localRotation = Quaternion.Euler(0, 0, imageRotation);
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

    private IEnumerator WaitForCameraPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);

        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));

        InitializeCamera();
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

    public void OpenCamera()
    {
        cameraSystemObject.gameObject.SetActive(true);
        playerSelectionObject.gameObject.SetActive(false);
    }


}
