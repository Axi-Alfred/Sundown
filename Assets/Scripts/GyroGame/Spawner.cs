using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawningBlocks;
    [SerializeField] private int timeBetweenSpawn;


    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(ReturnSpawnPositionX(), transform.position.y, 0);
        Instantiate(spawningBlocks, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int ReturnSpawnPositionX()
    {
        return Random.Range(-5, 5);
    }

    
}
