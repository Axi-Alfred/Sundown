using UnityEngine;
using System.Collections;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnInterval = 0.5f;
    public float minSpeed = 1f, maxSpeed = 3f;
    public float minScale = 0.5f, maxScale = 1.5f;

    public float spawnWidth = 5f;
    public float spawnYPosition = -6f; // Spawn off-screen below

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBalloon()
    {
        float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

        float scale = Random.Range(minScale, maxScale);
        balloon.transform.localScale = Vector3.one * scale;

        float verticalSpeed = Random.Range(minSpeed, maxSpeed);
        balloon.GetComponent<Balloon>().verticalSpeed = verticalSpeed;
    }
}
