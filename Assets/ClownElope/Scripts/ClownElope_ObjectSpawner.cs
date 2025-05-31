using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownElope_ObjectSpawner : MonoBehaviour
{
    private ClownElopeManager gameManager;

    public GameObject objectToSpawn; // Prefab att spawna

    public float spawnRate = 2f; // Tid mellan spawns
    public float spawnAcceleration = 0.05f; // Gör att spawn blir snabbare över tid
    private float timer = 0f; // Timer för spawn

    public float objectSpeed = 2f; // Startfart för objekten
    public float speedAcceleration = 0.1f; // Gör att objekten blir snabbare över tid

    public Sprite[] clownSprites; // Array med olika clowngrafiker

    void Start()
    {
        // Hämta referens till game manager
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

    void Update()
    {
        // Kolla om spelet är över
        if (gameManager != null && gameManager.IsGameOver())
        {
            return;
        }

        // Räkna upp timer
        timer += Time.deltaTime;

        // Kolla om det är dags att spawna
        if (timer >= spawnRate)
        {
            SpawnObject();
            timer = 0f;

            // Minska spawn rate för att öka svårigheten
            spawnRate -= spawnAcceleration;
            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 2f);
        }
    }

    void SpawnObject()
    {
        // Skapa objektet i mitten
        GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        // Välj en slumpad clowngrafik
        if (clownSprites.Length > 0)
        {
            Sprite randomSprite = clownSprites[Random.Range(0, clownSprites.Length)];
            SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = randomSprite;
            }
        }

        // Ge objektet en fart i slumpad riktning
        Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.velocity = randomDirection * objectSpeed;
        }

        // Öka objektets fart över tid för att öka svårigheten
        objectSpeed += speedAcceleration;
        objectSpeed = Mathf.Clamp(objectSpeed, 2f, 10f);
    }
}