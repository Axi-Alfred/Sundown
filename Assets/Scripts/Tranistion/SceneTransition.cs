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
    public Material circleFadeMaterial;
    public float fadeDuration = 1.5f;
    public float messageDuration = 3f;

    private string targetSceneName;
    private bool useMessage;
    private string messageText;

    private Material runtimeMat;
    private float fadeStartTime;
    private bool fadingIn = false;
    private bool fadingOut = false;
    private float fadeOutStartTime;

    public void Initialize()
    {
        var img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("[SceneTransition] No UI.Image component found.");
            return;
        }

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

        runtimeMat = new Material(circleFadeMaterial);
        runtimeMat.SetFloat("_Cutoff", 0f);
        img.material = runtimeMat;
        img.enabled = true;

        if (messageUI != null)
            messageUI.gameObject.SetActive(false);

        StartCoroutine(StartFadeNextFrame());
    }

    private IEnumerator StartFadeNextFrame()
    {
        yield return null;
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        if (runtimeMat == null) return;
        fadeStartTime = Time.realtimeSinceStartup;
        fadingIn = true;
        runtimeMat.SetFloat("_Cutoff", 0f);
    }

    public void StartFadeOut(string sceneName)
    {
        if (runtimeMat == null) return;
        if (BGMPlayer.Instance != null) BGMPlayer.Instance.FadeOutMusic();
        targetSceneName = sceneName;
        fadingOut = true;
        fadeOutStartTime = Time.realtimeSinceStartup;
        runtimeMat.SetFloat("_Cutoff", 1.5f);
    }

    private void Update()
    {
        if (fadingIn && runtimeMat != null)
        {
            float elapsed = Time.realtimeSinceStartup - fadeStartTime;
            float cutoff = Mathf.Lerp(0f, 1.5f, elapsed / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);

            if (elapsed >= fadeDuration)
            {
                runtimeMat.SetFloat("_Cutoff", 1.5f);
                fadingIn = false;
            }
        }

        if (fadingOut && runtimeMat != null)
        {
            float elapsed = Time.realtimeSinceStartup - fadeOutStartTime;
            float cutoff = Mathf.Lerp(1.5f, 0f, elapsed / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);

            if (elapsed >= fadeDuration)
            {
                runtimeMat.SetFloat("_Cutoff", 0f);
                fadingOut = false;
                LoadSceneAfterTransition();
            }
        }
    }

    private void LoadSceneAfterTransition()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
