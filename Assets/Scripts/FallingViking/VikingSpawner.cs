using UnityEngine;

public class VikingSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] vikingPrefabs; // Assign multiple Viking prefabs in the Inspector
    public GameObject axePrefab;

    [Header("Settings")]
    public float spawnInterval = 1.5f;
    public float xRange = 7f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFaller), 1f, spawnInterval);
    }

    void SpawnFaller()
    {
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);

        GameObject faller;

        // 80% chance to spawn a random Viking, 20% chance to spawn an Axe
        if (Random.value < 0.8f)
        {
            // Randomly pick one of the Viking prefabs
            int index = Random.Range(0, vikingPrefabs.Length);
            faller = Instantiate(vikingPrefabs[index], spawnPos, Quaternion.identity);
        }
        else
        {
            faller = Instantiate(axePrefab, spawnPos, Quaternion.identity);
        }

        Rigidbody2D rb = faller.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.angularVelocity = Random.Range(-360f, 360f);
        }
    }
}
