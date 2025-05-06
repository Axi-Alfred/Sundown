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
    public float spawnHeightOffset = -1f; // How far below screen to spawn
    public int maxActiveBalloons = 30;

    [Range(0f, 1f)]
    public float blackBalloonChance = 0.1f;

    private float spawnYPosition;

    void Start()
    {
        // Spawn balloons at the middle of the screen (Y-center)
        Camera cam = Camera.main;
        Vector3 middleOfScreen = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        spawnYPosition = middleOfScreen.y;

        StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Balloon").Length < maxActiveBalloons)
                SpawnBalloon();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBalloon()
    {
        float randomX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        balloon.tag = "Balloon"; // Ensure correct tag for tracking

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
