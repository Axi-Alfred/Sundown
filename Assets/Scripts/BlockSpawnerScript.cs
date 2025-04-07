using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{

    public GameObject block;

    //public float spawnRate;  //First iteration
    public float baseSpawnRate = 2f; //Second iteration

    public float widthOffset = 10f;

    private float timer;

    private float currentSpawnRate; //Second iteration



    void Start()
    {
        currentSpawnRate = baseSpawnRate; //Second iteration
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnBlock();
            timer = 0;
            // Gradually increase spawn rate
            currentSpawnRate = baseSpawnRate / (DifficultyManagerScript.Instance.CurrentMultiplier * 0.5f);
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
    void SpawnBlock()
    {
        float xPos = Random.Range(
            transform.position.x - widthOffset,
            transform.position.x + widthOffset
        );
        Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
    }

    /*float leftRange = transform.position.x - widthOffset;
    float rightRange = transform.position.x + widthOffset;

    Instantiate(block, new Vector3(Random.Range(leftRange, rightRange), transform.position.y, 0), transform.rotation);
    */
}
