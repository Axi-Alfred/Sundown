using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript1 : MonoBehaviour
{

    public GameObject block;

    //public float spawnRate;  //First iteration
    public float baseSpawnRate = 2f; //Second iteration

    public float widthOffset = 10f;

    [Header("Multi-Block Settings")]
    public int baseBlocksPerSpawn = 1; // Start 1 block
    public int maxBlocksPerSpawn = 5;  // Max blocks per spawn
    public float blockSpacing = 2f;    // Space between blocks

    private float timer;

    private float currentSpawnRate; //Second iteration

    private int currentBlocksPerSpawn;



    void Start()
    {
        currentSpawnRate = baseSpawnRate; //Second iteration
        currentBlocksPerSpawn = baseBlocksPerSpawn; //Third 
        //SpawnBlock();
        SpawnBlockGroup();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnBlockGroup();
            timer = 0;
            // Gradually increase spawn rate
            currentSpawnRate = baseSpawnRate / DifficultyManagerScript.Instance.CurrentMultiplier;
            currentBlocksPerSpawn = Mathf.Min(
                baseBlocksPerSpawn + Mathf.FloorToInt(DifficultyManagerScript.Instance.CurrentMultiplier / 2),
                maxBlocksPerSpawn);
        }

        /*if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            SpawnBlock();
            timer = 0;
        }
        */
    }

    void SpawnBlockGroup()
    {
        float totalWidth = widthOffset * 2;
        float spacing = totalWidth / (currentBlocksPerSpawn + 1);

        for (int i = 0; i < currentBlocksPerSpawn; i++)
        {
            float xPos = transform.position.x - widthOffset + (spacing * (i + 1));
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }

    /*void SpawnBlock()
    {
        float xPos = Random.Range(
            transform.position.x - widthOffset,
            transform.position.x + widthOffset
        );
        Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
    }*/

    /*float leftRange = transform.position.x - widthOffset;
    float rightRange = transform.position.x + widthOffset;

    Instantiate(block, new Vector3(Random.Range(leftRange, rightRange), transform.position.y, 0), transform.rotation);
    */
}
