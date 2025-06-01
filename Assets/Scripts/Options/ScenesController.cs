using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [Header("Prefabs to Instantiate")]
    public GameObject fadePrefab;
    public GameObject landscapeEnforcerPrefab;
    public GameObject portraitEnforcerPrefab;
    public GameObject timerPrefab;

    public enum OrientationMode { None, Landscape, Portrait }

    [Header("Scene Settings")]
    public OrientationMode orientationMode = OrientationMode.None;

    [Header("Scene Transition Settings")]
    public string nextSceneName;
    public bool enableFadeIn = true;
    public bool enableFadeOut = true;
    public bool autoFadeOutOnTimerEnd = true;
    public float fadeInDuration = 1f;

    [Header("Timer Settings")]
    public bool enableTimer = false;
    public float timerDuration = 10f;
    public string timerEndMessage = "Time's up!";

    private SceneTransition fadeController;
    private VisualTimerBar gameTimer;
    private bool alreadyTransitioning = false;
    private bool delayTimerStart = false;

    void Start()
    {
        StartCoroutine(SceneSetupSequence());
    }

    void Update()
    {
   
    }

    private IEnumerator SceneSetupSequence()
    {
        GameObject fadeObj = null;

        if (fadePrefab != null && enableFadeIn)
        {
            Debug.Log("[ScenesController] Instantiating fade prefab for fade-in...");
            fadeObj = Instantiate(fadePrefab);
            fadeController = fadeObj.GetComponentInChildren<SceneTransition>();

            if (fadeController == null)
            {
                Debug.LogError("[ScenesController] Fade prefab does not contain SceneTransition.");
            }
            else
            {
                fadeController.Initialize(autoStartFadeIn: true);
                // Optional: Destroy after fade-in completes
                Destroy(fadeObj, fadeInDuration + 0.5f);
            }
        }

        else
        {
            Debug.LogWarning("[ScenesController] No fadePrefab assigned.");
        }

        // Handle orientation enforcement
        switch (orientationMode)
        {
            case OrientationMode.Landscape:
                if (landscapeEnforcerPrefab != null)
                {
                    var land = Instantiate(landscapeEnforcerPrefab);
                    var forcedLandscape = land.GetComponent<ForcedLandscape>();
                    yield return forcedLandscape.StartEnforcing();
                }
                break;

            case OrientationMode.Portrait:
                if (portraitEnforcerPrefab != null)
                {
                    var port = Instantiate(portraitEnforcerPrefab);
                    var forcePortrait = port.GetComponent<ForcePortrait>();
                    forcePortrait.Enforce();
                    delayTimerStart = true;
                }
                break;
        }

        // ❌ No longer destroy the fade object. It remains in the scene ready for fade-out.

        // Handle optional timer setup
        if (enableTimer && timerPrefab != null)
        {
            var timerObj = Instantiate(timerPrefab);
            gameTimer = timerObj.GetComponent<VisualTimerBar>();
            gameTimer.duration = timerDuration;
            gameTimer.endMessage = timerEndMessage;

            if (delayTimerStart)
            {
                StartCoroutine(StartTimerAfterDelay(1f));
            }
            else
            {
                gameTimer.StartTimerSequence();
            }

            if (autoFadeOutOnTimerEnd)
            {
                StartCoroutine(WaitForTimerAndEnd(timerDuration + 5f));
            }
        }
    }


    private IEnumerator WaitForTimerAndEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TriggerEndNow();
    }

    private IEnumerator StartTimerAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        gameTimer.StartTimerSequence();
    }

    public void EndGameAndFadeOut()
    {
        if (alreadyTransitioning) return;
        alreadyTransitioning = true;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("[ScenesController] nextSceneName was EMPTY. Defaulting to 'Wheel'.");
            nextSceneName = "Wheel";
        }

        Debug.Log("[ScenesController] Fading out to: " + nextSceneName);

        if (enableFadeOut && fadePrefab != null)
        {
            GameObject fadeObj = Instantiate(fadePrefab);
            SceneTransition fadeController = fadeObj.GetComponentInChildren<SceneTransition>();

            if (fadeController != null)
            {
                fadeController.Initialize(autoStartFadeIn: false); // set up, but don’t fade in
                fadeController.StartFadeOut(nextSceneName);        // directly trigger fade-out
            }
            else
            {
                Debug.LogError("[ScenesController] Instantiated fade prefab is missing SceneTransition.");
                SceneManager.LoadScene(nextSceneName); // fallback
            }
        }
        else
        {
            Debug.LogWarning("[ScenesController] Fade disabled or prefab missing. Loading scene directly.");
            SceneManager.LoadScene(nextSceneName);
        }
    }


    public void TriggerEndNow()
    {
        if (!alreadyTransitioning)
        {
            Debug.Log("[ScenesController] TriggerEndNow called.");
            EndGameAndFadeOut();
        }
    }
}
