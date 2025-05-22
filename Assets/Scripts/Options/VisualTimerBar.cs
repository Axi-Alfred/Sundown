using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualTimerBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float duration = 10f;

    [Tooltip("Message to display when the timer ends")]
    public string endMessage = "Time's up!";

    [Header("UI References (Auto-Wired if Empty)")]
    public Image fillBar;
    public TextMeshProUGUI countdownText;
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    private float timeLeft;
    private bool timerRunning = false;
    private bool hasStarted = false;

    void Awake()
    {
        AutoWireUIReferences();
    }

    public void StartTimerSequence()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(SequenceBeforeTimer());
        }
    }

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.deltaTime;
        float t = Mathf.Clamp01(timeLeft / duration);

        if (fillBar != null)
            fillBar.fillAmount = t;

        if (timeLeft <= 0f)
        {
            timerRunning = false;
            ShowEndPanel();
        }
    }

    private System.Collections.IEnumerator SequenceBeforeTimer()
    {
        // Show countdown text if available
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            yield return CountdownRealTime();
            countdownText.gameObject.SetActive(false);
        }

        Time.timeScale = GameManager1.gameSpeedMultiplier;

        // Begin the timer
        timeLeft = duration;
        timerRunning = true;
    }

    private System.Collections.IEnumerator CountdownRealTime()
    {
        string[] steps = { "3", "2", "1", "Start!" };
        foreach (var step in steps)
        {
            if (countdownText != null)
                countdownText.text = step;

            yield return WaitForRealSeconds(1f);
        }
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
        {
            endPanel.SetActive(true);
            if (endText != null)
                endText.text = endMessage;
            GameManager1.EndTurn();
        }
    }

    private void AutoWireUIReferences()
    {
        if (fillBar == null)
            fillBar = GetComponentInChildren<Image>();

        if (countdownText == null)
            countdownText = GetComponentInChildren<TextMeshProUGUI>();

        if (endPanel == null)
        {
            Transform ep = transform.Find("EndPanel");
            if (ep != null) endPanel = ep.gameObject;
        }

        if (endText == null && endPanel != null)
        {
            endText = endPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
