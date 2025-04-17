using UnityEngine;

public class VikingSpawner : MonoBehaviour
{
    public GameObject vikingPrefab;
    public float spawnInterval = 1.5f;
    public float xRange = 8f;

    void Start()
    {
        InvokeRepeating("SpawnViking", 1f, spawnInterval);
    }

    void SpawnViking()
    {
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);
        Instantiate(vikingPrefab, spawnPosition, Quaternion.identity);
    }
}
