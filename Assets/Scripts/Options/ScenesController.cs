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
    public bool autoFadeOutOnTimerEnd = true;
    public float fadeInDuration = 0.5f;    // in seconds

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

        // 1. Instantiate fade and trigger fade-in internally via Initialize()
        if (fadePrefab != null)
        {
            fadeObj = Instantiate(fadePrefab);
            fadeController = fadeObj.GetComponentInChildren<SceneTransition>();
            fadeController.Initialize(); // ⬅️ this handles fade-in automatically
        }

        // 2. Enforce orientation (message shows behind fade)
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
                }
                break;
        }

        // ✅ DO NOT call fadeController.StartFadeIn(); again

        // 3. Destroy fade object if needed
        if (fadeObj != null)
            Destroy(fadeObj, fadeInDuration + 0.1f);

        // 4. Start timer
        if (timerPrefab != null)
        {
            var timerObj = Instantiate(timerPrefab);
            gameTimer = timerObj.GetComponent<VisualTimerBar>();
            gameTimer.duration = timerDuration;
            gameTimer.endMessage = timerEndMessage;
            gameTimer.StartTimerSequence(); // ✅ This triggers countdown + timer

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
            fadeController.StartFadeOut(nextSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
