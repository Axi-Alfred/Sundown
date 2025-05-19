using System.Collections;
using UnityEngine;

public class BalloonsSpawner : MonoBehaviour
{
    public GameObject[] colorBalloons;
    public GameObject blackBalloonPrefab;

    public float spawnInterval = 1f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
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
        GameObject prefabToSpawn = (Random.value < 0.1f)
            ? blackBalloonPrefab
            : colorBalloons[Random.Range(0, colorBalloons.Length)];

        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
