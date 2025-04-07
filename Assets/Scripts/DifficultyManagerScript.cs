using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManagerScript : MonoBehaviour
{
    public static DifficultyManagerScript Instance;

    [Header("Difficulty Settings")]
    public float baseFallSpeed = 5f;
    public float speedIncreaseRate = 0.1f;
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

    public float GetCurrentSpeed()
    {
        return baseFallSpeed * currentMultiplier;
    }
}
