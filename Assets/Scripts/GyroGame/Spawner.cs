using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawningBlocks;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float spawnerOffsetFromTop;
    [SerializeField] private int spawnCountMax;

    private int spawnCount;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());

        print(Screen.height);
    }

    // Update is called once per frame
    void Update()
    {


    }

    private Vector2 ReturnSpawnPosition()
    {
        return new Vector2(Random.Range(-4.5f, 4.5f), CalculateSpawnerY());
    }

    private IEnumerator SpawnLoop()
    {
        while (spawnCount < spawnCountMax)
        {
            transform.position = ReturnSpawnPosition();
            Instantiate(spawningBlocks, transform.position, Quaternion.identity);
            spawnCount++;

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    private float CalculateSpawnerY()
    {
        Camera cam = Camera.main;
        int screenResX = Screen.width;
        int screenResY = Screen.height;
        Vector3 topOfScreenDisplayPos = new Vector3(screenResX - 1f, screenResY - 1f, 0);
        Vector3 topOfScreenWorldPos = cam.ScreenToWorldPoint(topOfScreenDisplayPos);
        return topOfScreenWorldPos.y - spawnerOffsetFromTop;

    }

}
