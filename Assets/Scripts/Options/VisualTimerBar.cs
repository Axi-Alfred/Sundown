using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VisualTimerBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float duration = 10f;

    [Header("UI References (Auto-Wired if Empty)")]
    public Image fillBar;
    public TextMeshProUGUI countdownText;
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    private float timeLeft;
    private bool timerRunning = false;
    private bool pausedByThisScript = false;
    private bool hasStarted = false;

    void Awake()
    {
        AutoWireUIReferences();
    }

    void Start()
    {
        
            StartTimerSequence();
        
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
            StartCoroutine(ShowEndPanel());
        }
    }

    private System.Collections.IEnumerator SequenceBeforeTimer()
    {
        yield return new WaitForSeconds(0.4f);

        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            pausedByThisScript = true;
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            yield return CountdownRealTime();
            countdownText.gameObject.SetActive(false);
        }

        if (pausedByThisScript)
        {
            Time.timeScale = 1 * GameManager1.gameSpeedMultiplier;
        }

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

    private IEnumerator ShowEndPanel()
    {
        if (endPanel != null)
        {
            endPanel.SetActive(true);
            if (endText != null)
                endText.text = "Time's up!";
            yield return new WaitForSeconds(1f);
            GameManager1.EndTurn();
        }
    }

    private void AutoWireUIReferences()
    {
        // Auto-wire from children if not assigned
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
