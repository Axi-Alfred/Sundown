using System.Collections;
using UnityEngine;

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
    public bool showMessageOnFadeOut = false;
    public string messageText = "Minigame Complete!";
    public bool autoFadeOutOnTimerEnd = true;
    public float fadeInDuration = 1f;

    [Header("Timer Settings")]
    public float timerDuration = 10f;
    public string timerEndMessage = "Time's up!";

    private SceneTransition fadeController;
    private VisualTimerBar gameTimer;
    private bool alreadyTransitioning = false;

    void Start()
    {
        StartCoroutine(SceneSetupSequence());
    }

    private IEnumerator SceneSetupSequence()
    {
        GameObject fadeObj = null;

        // 1. Instantiate fade, but DO NOT fade in yet
        if (fadePrefab != null)
        {
            fadeObj = Instantiate(fadePrefab);
            fadeController = fadeObj.GetComponent<SceneTransition>();
            fadeController.Initialize(); // ← sets material but doesn't start fade
        }

        // 2. Enforce orientation (UI behind fade)
        switch (orientationMode)
        {
            case OrientationMode.Landscape:
                if (landscapeEnforcerPrefab != null)
                {
                    var land = Instantiate(landscapeEnforcerPrefab);
                    var forcedLandscape = land.GetComponent<ForcedLandscape>();
                    yield return forcedLandscape.StartEnforcing(); // ⏳ waits 5s total
                }
                break;

            case OrientationMode.Portrait:
                if (portraitEnforcerPrefab != null)
                {
                    var port = Instantiate(portraitEnforcerPrefab);
                    var forcePortrait = port.GetComponent<ForcePortrait>();
                    forcePortrait.Enforce(); // instant
                }
                break;
        }

        // 3. Run fade-in (after orientation message is done)
        if (fadeController != null)

        // 4. Remove fade canvas
        if (fadeObj != null)
            Destroy(fadeObj);

        // 5. Instantiate timer and SKIP wait
        if (timerPrefab != null)
        {
            var timerObj = Instantiate(timerPrefab);
            gameTimer = timerObj.GetComponent<VisualTimerBar>();
            gameTimer.duration = timerDuration;
            gameTimer.endMessage = timerEndMessage;

            // ← skip delay and start immediately
            gameTimer.StartTimerNow();

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

    public void EndGameAndFadeOut()
    {
        if (alreadyTransitioning) return;
        alreadyTransitioning = true;

        if (fadeController != null)
        {
            fadeController.StartFadeOut(nextSceneName, showMessageOnFadeOut, messageText);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
