using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawningBlocks;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float spawnerOffsetFromTop;

    private int spawnCount;
    private float spawnerYPos;


    // Start is called before the first frame update
    void Start()
    {
        spawnerYPos = Camera.main.transform.position.y + Camera.main.orthographicSize;
        StartCoroutine(SpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {


    }

    private Vector2 ReturnSpawnPosition()
    {
        return new Vector2(Random.Range(-4.5f, 4.5f), spawnerYPos);
    }

    private IEnumerator SpawnLoop()
    {
        while (spawnCount < 10)
        {
            transform.position = ReturnSpawnPosition();
            Instantiate(spawningBlocks, transform.position, Quaternion.identity);
            spawnCount++;

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

}
