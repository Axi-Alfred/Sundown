using UnityEngine;
using System.Collections;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;

    [Header("Balloon Sprites")]
    public Sprite[] coloredSprites; // Red, green, blue, yellow
    public Sprite blackSprite;      // Naughty balloon 😈

    [Header("Spawn Settings")]
    public float spawnInterval = 0.5f;
    public float minScale = 0.5f, maxScale = 1.5f;
    public float spawnWidth = 5f;
    private float spawnYPosition;

    [Range(0f, 1f)]
    public float blackBalloonChance = 0.1f;

    void Start()
    {
        // Calculate spawn Y-position based on camera
        Camera cam = Camera.main;
        spawnYPosition = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)).y;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBalloon()
    {
        float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

        // Set random scale
        float scale = Random.Range(minScale, maxScale);
        balloon.transform.localScale = Vector3.one * scale;

        // Sprite + behavior setup
        SpriteRenderer sr = balloon.GetComponent<SpriteRenderer>();
        Balloon balloonScript = balloon.GetComponent<Balloon>();

        if (Random.value < blackBalloonChance)
        {
            sr.sprite = blackSprite;
            balloonScript.isNegative = true;
        }
        else
        {
            int index = Random.Range(0, coloredSprites.Length);
            sr.sprite = coloredSprites[index];
            balloonScript.isNegative = false;
        }
    }
}
