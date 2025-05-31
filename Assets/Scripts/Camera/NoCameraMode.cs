using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoCameraMode : MonoBehaviour
{
    [SerializeField] private RectTransform panelsRT;
    [SerializeField] private GameObject permissionsButton;
    [SerializeField] private GameObject continueButton;


    [SerializeField] private Filter[] filtersArray;

    [SerializeField] private GameObject playersListPanel;
    [SerializeField] private GameObject initialPanel;

    [SerializeField] private Texture2D basicFace;

    private List<Filter> tempFiltersList;

    private Filter currentFilter;

    [SerializeField] private float filterScaleMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        permissionsButton.SetActive(false);
        continueButton.SetActive(false);
        StartCoroutine(PanelsDOTween());

        initialPanel.SetActive(true);
        playersListPanel.SetActive(false);

        tempFiltersList = filtersArray.ToList();
        tempFiltersList = tempFiltersList.OrderBy(p => Random.value).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPermissionsButton() //Send the player to permissions page in the settings tehe
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                var packageName = currentActivity.Call<string>("getPackageName");
        
                var uriClass = new AndroidJavaClass("android.net.Uri");
                var uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null);
        
                var intentObject = new AndroidJavaObject("android.content.Intent", 
                    "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject);
                intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
        
                currentActivity.Call("startActivity", intentObject);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
#endif

        Application.Quit();
    }

    private IEnumerator PanelsDOTween()
    {
        panelsRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => panelsRT.gameObject.SetActive(true));
        textSequence.Append(panelsRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(panelsRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);

        yield return new WaitForSeconds(3);

        continueButton.SetActive(true);
        permissionsButton.SetActive(true);


        yield return null;
    }

    public void ShowPlayersList()
    {
        StartCoroutine(AssignIconsLoop());

    }

    private IEnumerator AssignIconsLoop()
    {
        for (int i = 0; i < PlayerData.playersArray.Length; i++)
        {
            currentFilter = tempFiltersList[i];
            var currentPlayer = PlayerData.playersArray[i];
            currentPlayer.PlayerName = currentFilter.filterName;
            currentPlayer.PlayerIcon = MergeImages(basicFace, currentFilter.filterSprite);
        }

        yield return null;

        playersListPanel.SetActive(true);
        initialPanel.SetActive(false);
    }

    private Sprite MergeImages(Texture2D baseTexture, Sprite filter)
    {
        Texture2D filterTexture = filter.texture;

        int textureWidth = baseTexture.width;
        int textureHeight = baseTexture.height;

        int filterWidth = Mathf.RoundToInt(filterTexture.width * filterScaleMultiplier);
        int filterHeight = Mathf.RoundToInt(filterTexture.height * filterScaleMultiplier);

        Texture2D scaledFilter = ScaleTexture(filterTexture, filterWidth, filterHeight);
        Color[] filterPixels = scaledFilter.GetPixels();

        Texture2D result = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        result.SetPixels(baseTexture.GetPixels());

        int startX = (textureWidth - filterWidth) / 2;
        int startY = (textureHeight - filterHeight) / 2;

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
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);

        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }



}
