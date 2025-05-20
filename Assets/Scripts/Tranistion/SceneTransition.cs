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

    public void Initialize()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("[SceneTransition] Missing Image component.");
            return;
        }

        if (circleFadeMaterial == null && image.material == null)
        {
            Debug.LogError("[SceneTransition] No fade material assigned.");
            return;
        }

        // Use assigned material or fallback
        Material baseMat = circleFadeMaterial != null ? circleFadeMaterial : image.material;
        runtimeMat = new Material(baseMat);
        runtimeMat.SetFloat("_Cutoff", 0f);
        image.material = runtimeMat;
        image.enabled = true;

        if (messageUI != null)
            messageUI.gameObject.SetActive(false);

        StartCoroutine(StartFadeNextFrame());
    }

    private IEnumerator StartFadeNextFrame()
    {
        yield return null; // delay one frame
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        if (runtimeMat == null) return;
        fadeStartTime = Time.realtimeSinceStartup;
        fadingIn = true;
        runtimeMat.SetFloat("_Cutoff", 0f);
        Debug.Log("[SceneTransition] Starting fade-in...");
    }

    public void StartFadeOut(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneTransition] StartFadeOut called with empty scene name!");
            return;
        }

        if (runtimeMat == null)
        {
            Debug.LogError("[SceneTransition] Cannot start fade-out, runtimeMat is null.");
            return;
        }

        targetSceneName = sceneName;
        fadeStartTime = Time.realtimeSinceStartup;
        fadingOut = true;
        runtimeMat.SetFloat("_Cutoff", 1.5f);
        Debug.Log($"[SceneTransition] Starting fade-out to scene: {targetSceneName}");
    }

    private void Update()
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
                runtimeMat.SetFloat("_Cutoff", 0f);
                fadingOut = false;
                Debug.Log("[SceneTransition] Fade-out complete. Loading scene...");
                LoadSceneAfterTransition();
            }
        }
    }

    private void LoadSceneAfterTransition()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log($"[SceneTransition] Loading scene: {targetSceneName}");
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("[SceneTransition] Scene name is empty. Cannot load scene.");
        }
    }
}
