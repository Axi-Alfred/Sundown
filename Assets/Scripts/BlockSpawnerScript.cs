using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{

    public GameObject block;
    public float spawnRate;
    private float timer = 0;
    public float widthOffset = 10;

    // Start is called before the first frame update
    void Start()
    {
        spawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnBlock();
            timer = 0;
        }
    }
    void spawnBlock()
    {
        float leftRange = transform.position.x - widthOffset;
        float rightRange = transform.position.x + widthOffset;

        Instantiate(block, new Vector3(Random.Range(leftRange, rightRange), transform.position.y, 0), transform.rotation);
    }


}
