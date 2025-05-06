using UnityEngine;

public class WalkingClownSpawnerScript : MonoBehaviour
{
    [Header("Clown Setup")]
    public GameObject[] clownPrefabs;       // Olika clown-prefabs
    public Transform spawnPosition;         // Spawn-position
    public Animator kanonAnimator;          // För squish-animation (valfritt)

    [Header("Timing & Difficulty")]
    public float spawnInterval = 2f;        // Startintervall
    public float minSpawnInterval = 0.3f;   // Minsta tillåtna intervall
    public float difficultyRate = 0.05f;    // Hur snabbt det blir svårare

    [Header("Launch Force")]
    public float minForce = 5f;
    public float maxForce = 15f;
    public float minAngle = -30f;
    public float maxAngle = 30f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
        SpawnClown(); // Första direkt
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnClown();

            // Öka svårigheten gradvis
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - difficultyRate);

            timer = spawnInterval;
        }
    }

    void SpawnClown()
    {
        // Spela animation om finns
        if (kanonAnimator != null)
        {
            kanonAnimator.Play("KanonSquish", -1, 0f);
        }

        // Välj slumpmässig clown
        int index = Random.Range(0, clownPrefabs.Length);
        GameObject selectedClown = clownPrefabs[index];

        // Skapa clown
        GameObject clown = Instantiate(selectedClown, spawnPosition.position, Quaternion.identity);

        // Ge kraft om clown har Rigidbody2D
        Rigidbody2D rb = clown.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float force = Random.Range(minForce, maxForce);
            float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
