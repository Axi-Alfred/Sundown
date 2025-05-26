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
    private bool fadingIn = false;
    private bool fadingOut = false;

    private Material runtimeMat;
    private float fadeStartTime;
    private Image image;

    void Awake()
    {
        Initialize();
    }

    public void Initialize(bool autoStartFadeIn = true)
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("[SceneTransition] Missing Image component.");
            return;
        }

        Material baseMat = circleFadeMaterial != null ? circleFadeMaterial : image.material;
        runtimeMat = new Material(baseMat);
        runtimeMat.SetFloat("_Cutoff", 0f); // Start hidden
        image.material = runtimeMat;
        image.enabled = true;

        if (messageUI != null)
            messageUI.gameObject.SetActive(false);

        if (autoStartFadeIn)
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
        fadingIn = true;
        fadeStartTime = Time.realtimeSinceStartup;
        runtimeMat.SetFloat("_Cutoff", 0f);
        Debug.Log("[SceneTransition] Starting fade-in.");
    }

    public void StartFadeOut(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneTransition] StartFadeOut called with empty scene name.");
            return;
        }

        if (runtimeMat == null)
        {
            Debug.LogError("[SceneTransition] Cannot fade out, runtime material is null.");
            return;
        }

        targetSceneName = sceneName;
        fadingOut = true;
        fadeStartTime = Time.realtimeSinceStartup;
        runtimeMat.SetFloat("_Cutoff", 1.5f);
        Debug.Log("[SceneTransition] Starting fade-out to: " + targetSceneName);
    }

    void Update()
    {
        if (runtimeMat == null) return;

        float elapsed = Time.realtimeSinceStartup - fadeStartTime;

        if (fadingIn)
        {
            float cutoff = Mathf.Lerp(0f, 1.5f, elapsed / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);

            if (elapsed >= fadeDuration)
            {
                runtimeMat.SetFloat("_Cutoff", 1.5f);
                fadingIn = false;
                Debug.Log("[SceneTransition] Fade-in complete.");
            }
        }

        if (fadingOut)
        {
            float cutoff = Mathf.Lerp(1.5f, 0f, elapsed / fadeDuration);
            runtimeMat.SetFloat("_Cutoff", cutoff);

            if (elapsed >= fadeDuration)
            {
                fadingOut = false;
                Debug.Log("[SceneTransition] Fade-out complete. Loading: " + targetSceneName);
                LoadSceneAfterTransition();
            }
        }
    }

    private void LoadSceneAfterTransition()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("[SceneTransition] Target scene is empty!");
        }
    }
}
