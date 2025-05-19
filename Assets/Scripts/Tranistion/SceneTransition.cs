using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text messageUI;

    [Header("Fade Settings")]
    public Material circleFadeMaterial; // Assign in inspector (or fallback to Image's)
    public float fadeDuration = 1.5f;
    public float messageDuration = 3f;

    private string targetSceneName;
    private bool useMessage;
    private string messageText;
    private bool isFadingOut = false;

    private Material runtimeMat;

    /// <summary>
    /// Call this immediately after instantiating the prefab.
    /// </summary>
    public void Initialize()
    {
        var img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("[SceneTransition] No UI.Image component found.");
            return;
        }

        // TEMPORARILY disable the image to prevent premature rendering
        img.enabled = false;

        if (circleFadeMaterial == null && img.material == null)
        {
            Debug.LogError("[SceneTransition] No material assigned and no fallback available.");
            return;
        }

        if (circleFadeMaterial == null)
        {
            Debug.LogWarning("[SceneTransition] No material assigned — using image’s current material.");
            circleFadeMaterial = img.material;
        }

        // Create a fresh instance of the material and assign it
        runtimeMat = new Material(circleFadeMaterial);
        runtimeMat.SetFloat("_Cutoff", 0f); // 💣 Force black before render
        img.material = runtimeMat;

        // Now that material is safe, re-enable the Image
        img.enabled = true;

        // Start fade animation
        StartCoroutine(FadeIn());

        if (messageUI != null)
            messageUI.gameObject.SetActive(false);

        Debug.Log("[SceneTransition] Fade initialized and started with _Cutoff = 0");
    }


    public IEnumerator FadeIn()
    {
        if (runtimeMat == null)
        {
            Debug.LogError("[SceneTransition] FadeIn aborted — runtimeMat is null. Did you call Initialize()?");
            yield break;
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float cutoff = Mathf.Lerp(0f, 1.5f, timer / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

        runtimeMat.SetFloat("_Cutoff", 1.5f);
    }

    public void StartFadeOut(string sceneName, bool showMessage = false, string message = "Level Complete!")
    {
        targetSceneName = sceneName;
        useMessage = showMessage;
        messageText = message;

        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (runtimeMat == null)
        {
            Debug.LogError("[SceneTransition] FadeOut aborted — runtimeMat is null. Did you call Initialize()?");
            yield break;
        }

        if (useMessage && messageUI != null)
        {
            Time.timeScale = 0f;
            messageUI.text = messageText;
            messageUI.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(messageDuration);
            messageUI.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        isFadingOut = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float cutoff = Mathf.Lerp(1.5f, 0f, timer / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

        LoadSceneAfterTransition();
    }

    private void LoadSceneAfterTransition()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
