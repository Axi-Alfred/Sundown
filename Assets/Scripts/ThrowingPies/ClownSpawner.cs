using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject crateClownPrefab;
    public GameObject crateVisualPrefab;     // Optional: crate decoration
    public GameObject runnerClownPrefab;

    [Header("Settings")]
    public int crateSpawnCount = 3;
    public float spawnInterval = 3f;
    public float edgePadding = 0.5f;          // Padding to keep spawns inside screen

    void Start()
    {
        SpawnCratesAndClowns(); // One-time at start
        InvokeRepeating(nameof(SpawnRunnerClown), 1f, spawnInterval); // Looped
    }

    void SpawnCratesAndClowns()
    {
        Vector2 bounds = GetCameraBounds();
        float y = 0f; // Ground level

        // Calculate spacing within screen-safe zone
        float usableWidth = (bounds.x * 2f) - (edgePadding * 2f);
        float spacing = usableWidth / (crateSpawnCount - 1);

        for (int i = 0; i < crateSpawnCount; i++)
        {
            float x = -bounds.x + edgePadding + spacing * i;
            Vector2 spawnPos = new Vector2(x, y);

            Instantiate(crateClownPrefab, spawnPos, Quaternion.identity);

            if (crateVisualPrefab != null)
            {
                Instantiate(crateVisualPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    void SpawnRunnerClown()
    {
        Vector2 bounds = GetCameraBounds();
        float y = 0f;
        float x = Random.Range(-bounds.x + edgePadding, bounds.x - edgePadding);
        Vector2 spawnPos = new Vector2(x, y);

        GameObject clown = Instantiate(runnerClownPrefab, spawnPos, Quaternion.identity);

        // Flip only X-scale, preserving size
        float direction = (spawnPos.x > 0) ? -1f : 1f;
        Vector3 originalScale = clown.transform.localScale;
        originalScale.x = Mathf.Abs(originalScale.x) * direction;
        clown.transform.localScale = originalScale;

        clown.GetComponent<ClownRunner>().speed *= direction;
    }

    Vector2 GetCameraBounds()
    {
        float vertSize = Camera.main.orthographicSize;
        float horzSize = vertSize * Camera.main.aspect;
        return new Vector2(horzSize, vertSize);
    }
}
