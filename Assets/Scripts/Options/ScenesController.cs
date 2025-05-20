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
    [Tooltip("Choose which screen orientation to enforce")]
    public OrientationMode orientationMode = OrientationMode.None;

    [Header("Scene Transition Settings")]
    public string nextSceneName;
    public bool enableFadeIn = true;   // ✅ Toggle for fade-in
    public bool enableFadeOut = true;  // ✅ Toggle for fade-out
    public bool autoFadeOutOnTimerEnd = true;
    public float fadeInDuration = 0.5f;

    [Header("Timer Settings")]
    public bool enableTimer = true;    // ✅ Toggle for timer
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

    private IEnumerator SceneSetupSequence()
    {
        GameObject fadeObj = null;

        // 1. Instantiate fade and trigger fade-in
        if (fadePrefab != null)
        {
            fadeObj = Instantiate(fadePrefab);
            fadeController = fadeObj.GetComponentInChildren<SceneTransition>();

            if (enableFadeIn && fadeController != null)
            {
                if (orientationMode == OrientationMode.Portrait)
                {
                    yield return new WaitForSecondsRealtime(1f);
                    fadeController.Initialize();
                }
                else
                {
                    fadeController.Initialize();
                }
            }
        }

        // 2. Enforce orientation
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

        // 3. Destroy fade object if needed
        if (enableFadeIn && fadeObj != null)
            Destroy(fadeObj, fadeInDuration + 0.1f);

        // 4. Start timer if enabled
        if (enableTimer && timerPrefab != null)
        {
            var timerObj = Instantiate(timerPrefab);
            gameTimer = timerObj.GetComponent<VisualTimerBar>();
            gameTimer.duration = timerDuration;
            gameTimer.endMessage = timerEndMessage;

            if (delayTimerStart)
                StartCoroutine(StartTimerAfterDelay(1f));
            else
                gameTimer.StartTimerSequence();

            if (autoFadeOutOnTimerEnd)
                StartCoroutine(WaitForTimerAndEnd(timerDuration + 5f));
        }
    }

    private IEnumerator WaitForTimerAndEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (!alreadyTransitioning)
        {
            alreadyTransitioning = true;
            EndGameAndFadeOut();
        }
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

        if (enableFadeOut && fadeController != null)
        {
            fadeController.StartFadeOut(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
