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

    [SerializeField] private Image faceTemplate;
    [SerializeField] private Sprite tempFilter;
    

    private WebCamTexture webcamTexture;
    private WebCamDevice webcamDevice;

    private float aspectRatio;
    private float imageRotation = -90;

    //Player specifics

    private Player currentPlayer;

    private void Start()
    {
        StartCoroutine(WaitForCameraPermission());
    }
    private void InitializeCamera()
    {
        faceTemplate.gameObject.SetActive(true);

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
        //rectTransform.localRotation = Quaternion.Euler(0, 0, imageRotation);
    }

    private void AdjustTakenImageAspectRatio()
    {
        RectTransform rectTransform = capturedImageDisplay.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspectRatio);
        rectTransform.localScale = new Vector3(-1, 1, 1);
        //rectTransform.localRotation = Quaternion.Euler(0, 0, imageRotation);
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

        faceTemplate.gameObject.SetActive(false);
        cameraPreview.gameObject.SetActive(false);
        capturedImageDisplay.sprite = MergeImages(new Vector2().normalized);
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

        faceTemplate.gameObject.SetActive(true);
        capturedImageDisplay.sprite = null;
        capturedImageDisplay.gameObject.SetActive(false);
        retakeButton.SetActive(false);
        takeButton.SetActive(true);
    }

    private Sprite MergeImages(Vector2 position)
    {
        Texture2D imageTexture = capturedImageDisplay.sprite.texture;
        Texture2D filterTexture = tempFilter.texture;

        int width = imageTexture.width;
        int height = imageTexture.height;

        Texture2D resultTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        resultTexture.SetPixels(imageTexture.GetPixels());

        Color[] filterPixels = filterTexture.GetPixels();
        int filterWidth = filterTexture.width;
        int filterHeight = filterTexture.height;

        for (int y = 0; y < filterHeight; y++)
        {
            for (int x = 0; x < filterWidth; x++)
            {
                int targetX = (int)position.x + x;
                int targetY = (int)position.y + y;

                if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height) continue;

                Color imageColor = resultTexture.GetPixel(targetX, targetY);
                Color filterColor = filterPixels[y * filterWidth + x];
                Color finalColor = Color.Lerp(imageColor, filterColor, filterColor.a);
                resultTexture.SetPixel(targetX, targetY, finalColor);
            }
        }

        resultTexture.Apply();

        Sprite result = Sprite.Create(resultTexture, new Rect(0, 0, resultTexture.width, resultTexture.height), new Vector2(0.5f, 0.5f));
        return result;
    }


}
