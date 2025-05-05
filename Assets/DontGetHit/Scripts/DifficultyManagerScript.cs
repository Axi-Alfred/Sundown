using UnityEngine;

public class DifficultyManagerScript : MonoBehaviour
{
    public static DifficultyManagerScript Instance;

    [Header("Blocktäthet")]
    public float blocksPerMinute = 30f; // Svårighetskurva

    [Header("Svårighetsinställningar")]
    public float baseFallSpeed = 5f; // Basfallhastighet
    public float speedIncreaseRate = 0.1f; // Hastighetsökning över tid
    [SerializeField] private float currentMultiplier = 1f;
    public float CurrentMultiplier => currentMultiplier;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        currentMultiplier += speedIncreaseRate * Time.deltaTime;
    }

    // Returnerar nuvarande fallhastighet
    public float GetCurrentSpeed()
    {
        return baseFallSpeed * currentMultiplier;
    }
}