using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    private ClownElopeManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

    public GameObject objectToSpawn;

    public float spawnRate = 2f;
    public float spawnAcceleration = 0.05f; // makes spawn quicker
    private float timer = 0f;

    public float objectSpeed = 2f;
    public float speedAcceleration = 0.1f; // makes objects faster

    void Update()
    {
        if (gameManager != null && gameManager.IsGameOver())
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnObject();
            timer = 0f;

            spawnRate -= spawnAcceleration;
            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 2f);
        }
    }


    void SpawnObject()
    {
        // Spawn the object
        GameObject spawned = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        // Set speed and increase it gradually
        Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.velocity = randomDirection * objectSpeed;
        }

        // Gradually increase speed
        objectSpeed += speedAcceleration;
        objectSpeed = Mathf.Clamp(objectSpeed, 2f, 10f); // Limit maximum speed
    }
}
