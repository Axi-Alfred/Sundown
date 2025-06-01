using UnityEngine;

// Skript som hanterar att skapa nya objekt (clowner) under spelets gång
public class ClownElopeObjectSpawner : MonoBehaviour
{
    // Referens till spelets manager för att kunna kolla om spelet är över
    private ClownElopeManager gameManager;

    // Prefab som ska skapas varje gång
    public GameObject objectToSpawn;

    // Hur ofta (i sekunder) ett nytt objekt ska skapas
    public float spawnRate = 2f;

    // Hur mycket spawnRate minskar varje gång för att öka svårighetsgraden
    public float spawnAcceleration = 0.05f;

    // Timer som håller koll på tiden mellan spawn
    private float timer = 0f;

    // Startfart för nya objekt
    public float objectSpeed = 2f;

    // Hur mycket objektets fart ökar efter varje spawn
    public float speedAcceleration = 0.1f;

    // Array med olika utseenden på clowner
    public Sprite[] clownSprites;

    // Start körs en gång när spelet startar
    void Start()
    {
        // Hitta referensen till spelets manager
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

    // Update körs en gång per frame
    void Update()
    {
        // Om spelet är över, gör ingenting
        if (gameManager != null && gameManager.IsGameOver()) return;

        // Lägg till tiden sedan förra framen till timern
        timer += Time.deltaTime;

        // Om timern har nått spawnRate ska vi skapa ett nytt objekt
        if (timer >= spawnRate)
        {
            SpawnObject(); // Skapa nytt objekt
            timer = 0f; // Nollställ timern

            // Minska spawnRate för att öka svårighetsgraden
            spawnRate = spawnRate - spawnAcceleration;

            // Se till att spawnRate inte blir för liten
            if (spawnRate < 0.3f)
            {
                spawnRate = 0.3f;
            }
        }
    }

    // Funktion som skapar ett nytt objekt
    void SpawnObject()
    {
        // Skapa nytt objekt vid spawnerns position
        GameObject newObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        // Byt ut objektets sprite till en slumpmässig clown (om det finns sprites)
        if (clownSprites.Length > 0)
        {
            Sprite randomSprite = clownSprites[Random.Range(0, clownSprites.Length)];
            SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = randomSprite;
            }
        }

        // Ge objektet en riktning och fart
        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Slumpa en riktning (X och Y mellan -1 och 1)
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            // Skapa en normaliserad vektor för jämn fart
            Vector2 direction = new Vector2(x, y).normalized;

            // Ge objektet en fart i den riktningen
            rb.velocity = direction * objectSpeed;
        }

        // Öka objektets fart för nästa spawn
        objectSpeed = objectSpeed + speedAcceleration;

        // Se till att farten inte blir för hög
        if (objectSpeed > 10f)
        {
            objectSpeed = 10f;
        }
    }
}