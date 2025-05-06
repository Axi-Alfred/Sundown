using UnityEngine;

public class DifficultyManagerScript : MonoBehaviour
{
    public static DifficultyManagerScript Instance;

    [Header("Sv�righetsinst�llningar")]
    public float baseFallSpeed = 5f;
    public float speedIncreaseRate = 0.1f;
    public float blocksPerMinute = 30f;

    private float currentMultiplier = 1f;

    // Tillg�ngligg�r currentMultiplier f�r andra klasser
    public float GetCurrentMultiplier()
    {
        return currentMultiplier;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // �ka sv�righetsgraden �ver tid
        currentMultiplier += speedIncreaseRate * Time.deltaTime;
    }

    // Returnerar nuvarande fallhastighet
    public float GetCurrentSpeed()
    {
        return baseFallSpeed * currentMultiplier;
    }
}