using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VisualTimerBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float duration = 10f;         // Countdown time in seconds
    public float startDelay = 3f;        // Delay before timer starts

    [Header("References")]
    public Image fillBar;                // UI Fill image

    private float timeLeft;
    private float delayLeft;
    private bool timerStarted = false;
    private bool sceneLoaded = false;

    void OnEnable()
    {
        delayLeft = startDelay;
        timeLeft = duration;
        fillBar.fillAmount = 1f;
    }

    void Update()
    {
        // Wait during delay phase
        if (!timerStarted)
        {
            delayLeft -= Time.deltaTime;
            if (delayLeft <= 0f)
                timerStarted = true;

            return; // Don’t start timer yet
        }

        // Timer running
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            float t = Mathf.Clamp01(timeLeft / duration);
            fillBar.fillAmount = t;
        }
        else if (!sceneLoaded)
        {
            sceneLoaded = true;
            GameManager1.EndRound();
        }
    }
}
