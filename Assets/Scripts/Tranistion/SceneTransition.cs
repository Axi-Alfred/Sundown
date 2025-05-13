using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public static Animator animator;
    private static object nextSceneToLoad;
    public static bool sceneHasLoaded;

    [Header("Optional Message Before Fade")]
    [Tooltip("If true, a message will appear before fade-out.")]
    public bool showMessage = false;

    [Tooltip("The message to display on screen (e.g. 'You Win!')")]
    public string messageText = "Level Complete!";

    [Tooltip("UI Text component used to show the message.")]
    public TMP_Text messageUI;

    [Tooltip("Duration the message is shown before fading (in seconds)")]
    public float messageDuration = 3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
        sceneHasLoaded = true;

        if (messageUI != null)
            messageUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Call this to begin a transition to a scene, with optional pause/message.
    /// </summary>
    public static void FadeOut(object scene)
    {
        nextSceneToLoad = scene;
        instance.StartCoroutine(instance.FadeOutWithMessage());
    }

    // Reference to self (set in Awake)
    private static SceneTransition instance;

    private void Start()
    {
        instance = this;
    }

    private IEnumerator FadeOutWithMessage()
    {
        if (showMessage && messageUI != null)
        {
            Time.timeScale = 0f; // Pause game
            messageUI.text = messageText;
            messageUI.gameObject.SetActive(true);

            yield return StartCoroutine(WaitForRealtimeSeconds(messageDuration));

            messageUI.gameObject.SetActive(false);
            Time.timeScale = 1f; // Resume game
        }

        animator.SetTrigger("FadeOut");
    }

    private IEnumerator WaitForRealtimeSeconds(float seconds)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + seconds)
        {
            yield return null;
        }
    }

    // Called via Animation Event at the end of FadeOut animation
    public void LoadSceneAfterTransition()
    {
        if (nextSceneToLoad is string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (nextSceneToLoad is int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        sceneHasLoaded = false;
    }
}
