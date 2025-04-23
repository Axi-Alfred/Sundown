using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualTimerBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float duration = 10f;

    [Header("References")]
    public Image fillBar;
    public TextMeshProUGUI countdownText;
    public GameObject endPanel;

    private float timeLeft;
    private bool timerStarted = false;
    private bool sceneEnded = false;

    // Call this method from another script to start the countdown
    public void Begin()
    {
        StartCoroutine(CountdownRoutine());
    }

    void Update()
    {
        if (!timerStarted || sceneEnded) return;

        timeLeft -= Time.deltaTime;
        float t = Mathf.Clamp01(timeLeft / duration);
        fillBar.fillAmount = t;

        if (timeLeft <= 0f && !sceneEnded)
        {
            sceneEnded = true;
            ShowEndPanel();
        }
    }

    private System.Collections.IEnumerator CountdownRoutine()
    {
        // Freeze the game during countdown
        Time.timeScale = 0f;

        countdownText.gameObject.SetActive(true);
        countdownText.text = "3"; yield return WaitForRealSeconds(1f);
        countdownText.text = "2"; yield return WaitForRealSeconds(1f);
        countdownText.text = "1"; yield return WaitForRealSeconds(1f);
        countdownText.text = "Start!"; yield return WaitForRealSeconds(1f);

        countdownText.gameObject.SetActive(false);

        // Resume game
        Time.timeScale = 1f;
        timeLeft = duration;
        timerStarted = true;
    }

    private System.Collections.IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    private void ShowEndPanel()
    {
        if (endPanel != null)
            endPanel.SetActive(true);

        GameManager1.EndRound();
    }
}
