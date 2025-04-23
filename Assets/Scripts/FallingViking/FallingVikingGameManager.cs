using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FallingVikingGameManager : MonoBehaviour
{
    public static FallingVikingGameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
