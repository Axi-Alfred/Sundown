using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{
    public GameObject block;
    public float baseSpawnRate = 1.5f;
    public float widthOffset = 10f;
    public float minBlockSpacing = 1.5f;

    private float timer = 0f;
    private float currentSpawnRate;
    private int currentBlocksPerWave = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        SpawnBlockWave();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnBlockWave();
            timer = 0f;

            // Update difficulty
            float currentMultiplier = DifficultyManagerScript.Instance.GetCurrentMultiplier();
            currentSpawnRate = baseSpawnRate / currentMultiplier;

            // Calculate number of blocks per wave
            int estimatedBlocks = Mathf.FloorToInt(currentMultiplier / 0.5f);

            // Clamp to 1?5 blocks
            currentBlocksPerWave = Mathf.Clamp(estimatedBlocks, 1, 5);
        }
    }

    // Spawns a wave of blocks
    void SpawnBlockWave()
    {
        List<float> usedPositions = new List<float>();
        int safetyCounter = 0;

        // Add center position (could be player position)
        usedPositions.Add(0f);

        for (int i = 0; i < currentBlocksPerWave; i++)
        {
            bool validPosition = false;
            float xPos = 0f;

            while (!validPosition && safetyCounter < 100)
            {
                validPosition = true;

                xPos = Random.Range(
                    transform.position.x - widthOffset,
                    transform.position.x + widthOffset
                );

                foreach (float used in usedPositions)
                {
                    if (Mathf.Abs(xPos - used) < minBlockSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }

                safetyCounter++;
            }

            usedPositions.Add(xPos);
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }
}
