using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingClownSpawnerScript : MonoBehaviour
{
    public GameObject clownPrefab;         //  Prefab för clownen
    public Transform spawnPoint;           //  Plats där clownen spawna
    public float spawnInterval = 2f;       //  Startintervall
    public float minSpawnInterval = 0.3f;  //  Minsta tillåtna intervall
    public float difficultyRate = 0.05f;   // ⬇ Hur mycket intervallet minskar varje gång

    private float timer;

    void Start()
    {
        timer = spawnInterval;
        SpawnClown(); //  Spawna första clownen direkt
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnClown();

            // 🛠 Minska intervallet tills det når minimum
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - difficultyRate);

            timer = spawnInterval;
        }
    }

    void SpawnClown()
    {
        Instantiate(clownPrefab, spawnPoint.position, Quaternion.identity);
    }
}
