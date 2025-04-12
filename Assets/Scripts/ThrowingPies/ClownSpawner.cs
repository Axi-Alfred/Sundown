using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownSpawner : MonoBehaviour
{
    public GameObject clownPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnClown), 1f, spawnInterval);
    }

    void SpawnClown()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(clownPrefab, spawnPoints[index].position, Quaternion.identity);
    }
}

