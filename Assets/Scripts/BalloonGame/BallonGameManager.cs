using UnityEngine;
using System.Collections;

public class BalloonGameManager : MonoBehaviour
{
    public static BalloonGameManager Instance;

    [Header("Balloon Prefabs")]
    public GameObject[] colorBalloons; // Assign the 4 colored balloon prefabs
    public GameObject blackBalloonPrefab;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints; // Now an array of 3 spawn positions
    public float spawnInterval = 0.75f; // Faster spawn

    [Header("Pause Effect")]
    public GameObject blackScreenOverlay; // Fullscreen UI image

    [Header("Tracking")]
    public int poppedBalloons = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
        blackScreenOverlay.SetActive(false);
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBalloon()
    {
        Transform chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector2 spawnPos = chosenSpawn.position;

        GameObject prefabToSpawn = (Random.value < 0.1f)
            ? blackBalloonPrefab
            : colorBalloons[Random.Range(0, colorBalloons.Length)];

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
    public void RegisterBalloonPop()
    {
        poppedBalloons++;

        if (poppedBalloons >= 20)
        {
            poppedBalloons = 0;
            PlayerData.currentPlayerTurn.AddScore(1);
            Debug.Log("🎉 Score awarded!");
            GameManager1.EndTurn(); // ✅ Go to Wheel scene
        }
    }



    public void TriggerBlackBalloon()
    {
        StartCoroutine(PauseGameWithOverlay());
    }

    IEnumerator PauseGameWithOverlay()
    {
        Time.timeScale = 0f;
        blackScreenOverlay.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        blackScreenOverlay.SetActive(false);
        Time.timeScale = 1f;
    }
}
