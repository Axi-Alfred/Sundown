using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class FrontCamera : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private RawImage cameraPreview;
    [SerializeField] private Image capturedImageDisplay;
    [SerializeField] private GameObject cameraSystemObject;

    [SerializeField] private Image faceTemplate;
    [SerializeField] private float filterScale = 0.75f;

    private Sprite tempFilterSprite;
    private IconsManager iconsManager;


    private WebCamTexture webcamTexture;
    private WebCamDevice webcamDevice;

    private float aspectRatio;
    private float imageRotation = -90;

    private void Start()
    {
        iconsManager = GetComponent<IconsManager>();

        StartCoroutine(WaitForCameraPermission()); //Before initializing you need to first check permission using this coroutine
    }
    private void InitializeCamera()
    {
        faceTemplate.gameObject.SetActive(true);

        foreach (var i in WebCamTexture.devices)
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

    private void AdjustTakenImageAspectRatio(Texture2D picTexture)
    {
        float newAspectRatio = (float)picTexture.width / (float)picTexture.height;
        RectTransform rectTransform = capturedImageDisplay.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * newAspectRatio);
    }

    public IEnumerator TakePicture()
    {
        capturedImageDisplay.gameObject.SetActive(true);

        //Filter initiation
        tempFilterSprite = iconsManager.currentFilter.filterSprite;

        yield return new WaitForEndOfFrame();

        Color[] pixels = webcamTexture.GetPixels();
        Texture2D picTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
        picTexture.SetPixels(pixels);
        picTexture.Apply();

        picTexture = RotateTexture(picTexture, clockwise: true);
        picTexture = FlipTextureVertically(picTexture);

        Sprite rotatedSprite = Sprite.Create(picTexture, new Rect(0, 0, picTexture.width, picTexture.height), new Vector2(0.5f, 0.5f));
        capturedImageDisplay.sprite = rotatedSprite;

        AdjustTakenImageAspectRatio(picTexture);

        //Could potentially play some extra animation after the pic is taken before the filter is applied

        capturedImageDisplay.sprite = MergeImages(picTexture);

        iconsManager.currentPlayer.PlayerIcon = capturedImageDisplay.sprite;
        iconsManager.currentPlayer.PlayerName = iconsManager.currentFilter.name;

        faceTemplate.gameObject.SetActive(false);
        cameraPreview.gameObject.SetActive(false);
    }

    private IEnumerator WaitForCameraPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);

        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));

        InitializeCamera();
    }

    public void RetakePictureButton()
    {
        cameraPreview.gameObject.SetActive(true);

        faceTemplate.gameObject.SetActive(true);
        capturedImageDisplay.sprite = null;
        capturedImageDisplay.gameObject.SetActive(false);
    }

    private Sprite MergeImages(Texture2D baseTexture)
    {
        Texture2D filterTexture = tempFilterSprite.texture;

        int textureWidth = baseTexture.width;
        int textureHeight = baseTexture.height;

        RectTransform capturedRect = capturedImageDisplay.rectTransform;
        RectTransform faceRect = faceTemplate.rectTransform;

        float relativeWidth = faceRect.rect.width / capturedRect.rect.width;
        float relativeHeight = faceRect.rect.height / capturedRect.rect.height;

        int filterWidth = Mathf.RoundToInt(textureWidth * relativeWidth * filterScale);
        int filterHeight = Mathf.RoundToInt(textureHeight * relativeHeight * filterScale);

        Texture2D scaledFilter = ScaleTexture(filterTexture, filterWidth, filterHeight);
        Color[] filterPixels = scaledFilter.GetPixels();

        int startX = (textureWidth - filterWidth) / 2;
        int startY = (textureHeight - filterHeight) / 2;

        Texture2D result = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        result.SetPixels(baseTexture.GetPixels());

        for (int y = 0; y < filterHeight; y++)
        {
            for (int x = 0; x < filterWidth; x++)
            {
                int targetX = startX + x;
                int targetY = startY + y;

                if (targetX < 0 || targetX >= textureWidth || targetY < 0 || targetY >= textureHeight) continue;

                Color baseColor = result.GetPixel(targetX, targetY);
                Color filterColor = filterPixels[y * filterWidth + x];
                Color finalColor = Color.Lerp(baseColor, filterColor, filterColor.a);

                result.SetPixel(targetX, targetY, finalColor);
            }
        }

        result.Apply();

        return Sprite.Create(result, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();
        RenderTexture.ReleaseTemporary(rt);
        return result;

    }

    public static Texture2D RotateTexture(Texture2D original, bool clockwise)
    {
        int width = original.width;
        int height = original.height;
        Texture2D rotated = new Texture2D(height, width); 

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if (clockwise)
                    rotated.SetPixel(j, width - i - 1, original.GetPixel(i, j));
                else
                    rotated.SetPixel(height - j - 1, i, original.GetPixel(i, j));
            }
        }

        rotated.Apply();
        return rotated;
    }

    public static Texture2D FlipTextureVertically(Texture2D original)
    {
        int width = original.width;
        int height = original.height;

        Texture2D flipped = new Texture2D(width, height, original.format, false);

        for (int y = 0; y < height; y++)
        {
            flipped.SetPixels(0, y, width, 1, original.GetPixels(0, height - y - 1, width, 1));
        }

        flipped.Apply();
        return flipped;
    }

    //Ignore this method totally, its old and doesnt work, only here for veckoredovisning :)
    private Sprite Merge()
    {
        int width = tempFilterSprite.texture.width;
        int height = tempFilterSprite.texture.height;
        Texture2D tempBackgroundTexture = new Texture2D(width, height);

        Color[] filterPixels = tempFilterSprite.texture.GetPixels();
        Color[] newPixels = new Color[width * height]; 

        for (int i = 0; i < filterPixels.Length; i++)
        {

            if (filterPixels[i].a > 0)
            {
                newPixels[i] = filterPixels[i];
            }
        }

        tempBackgroundTexture.SetPixels(newPixels);
        tempBackgroundTexture.Apply();

        return Sprite.Create(tempBackgroundTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

    }
}
