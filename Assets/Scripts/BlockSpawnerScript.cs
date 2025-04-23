using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockSpawnerScript : MonoBehaviour
{
    public GameObject block;
    public float baseSpawnRate = 1.5f;
    public float widthOffset = 10f;
    public float minBlockSpacing = 1.5f;

    private float timer;
    private float currentSpawnRate;
    private int currentBlocksPerWave = 1;

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        SpawnWave();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnWave();
            timer = 0;

            // Increase difficulty
            currentSpawnRate = baseSpawnRate / DifficultyManagerScript.Instance.CurrentMultiplier;
            currentBlocksPerWave = Mathf.Clamp(
                Mathf.FloorToInt(DifficultyManagerScript.Instance.CurrentMultiplier / 0.5f),
                1,
                5
            );
        }
    }

    void SpawnWave()
    {
        List<float> usedPositions = new List<float>();
        int safetyCounter = 0;

        // Always check against the player's initial position
        usedPositions.Add(0f); // Player's starting X position

        for (int i = 0; i < currentBlocksPerWave; i++)
        {
            bool validPosition;
            float xPos;

            do
            {
                validPosition = true;
                xPos = Random.Range(
                    transform.position.x - widthOffset,
                    transform.position.x + widthOffset
                );

                // Check spacing against ALL existing positions (including player)
                foreach (float pos in usedPositions)
                {
                    if (Mathf.Abs(xPos - pos) < minBlockSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }

                safetyCounter++;
            }
            while (!validPosition && safetyCounter < 100);

            usedPositions.Add(xPos);
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockSpawnerScript : MonoBehaviour
{
    public GameObject block;
    public float baseSpawnRate = 1.5f;
    public float widthOffset = 10f;
    public float minBlockSpacing = 1.5f;

    private float timer;
    private float currentSpawnRate;
    private int currentBlocksPerWave = 1;

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        SpawnWave();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnWave();
            timer = 0;

            // Increase difficulty
            currentSpawnRate = baseSpawnRate / DifficultyManagerScript.Instance.CurrentMultiplier;
            currentBlocksPerWave = Mathf.Clamp(
                Mathf.FloorToInt(DifficultyManagerScript.Instance.CurrentMultiplier / 0.5f),
                1,
                5
            );
        }
    }

    void SpawnWave()
    {
        List<float> usedPositions = new List<float>();
        int safetyCounter = 0;

        // Always check against the player's initial position
        usedPositions.Add(0f); // Player's starting X position

        for (int i = 0; i < currentBlocksPerWave; i++)
        {
            bool validPosition;
            float xPos;

            do
            {
                validPosition = true;
                xPos = Random.Range(
                    transform.position.x - widthOffset,
                    transform.position.x + widthOffset
                );

                // Check spacing against ALL existing positions (including player)
                foreach (float pos in usedPositions)
                {
                    if (Mathf.Abs(xPos - pos) < minBlockSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }

                safetyCounter++;
            }
            while (!validPosition && safetyCounter < 100);

            usedPositions.Add(xPos);
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }
}
