using UnityEngine;
using TMPro;

public class BalloonGameManager : MonoBehaviour
{
    public static BalloonGameManager Instance;

    private int hiddenScore = 0;
    public int scoreNeededToWin = 20;

    public float gameDuration = 30f;
    private float timer;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timer = gameDuration;
        UpdateScoreText();
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timerText != null)
            timerText.text = $"Time: {Mathf.Ceil(timer)}";

        if (timer <= 0)
        {
            CheckWinCondition();
        }
    }

    public void BalloonPopped(bool isNegative)
    {
        if (gameEnded) return;

        if (isNegative)
            hiddenScore = Mathf.Max(0, hiddenScore - 2);
        else
            hiddenScore++;

        UpdateScoreText(); // make sure this updates UI
    }


    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {hiddenScore}";
    }

    public void CheckWinCondition()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (hiddenScore >= scoreNeededToWin)
        {
            Debug.Log("✅ You Win!");
            PlayerData.currentPlayerTurn.AddScore(1);
        }
        else
        {
            Debug.Log("❌ You Lose!");
        }

        // Stop spawning balloons
        BalloonSpawner spawner = FindObjectOfType<BalloonSpawner>();
        if (spawner) spawner.StopAllCoroutines();

        GameManager1.EndTurn();
    }
}
