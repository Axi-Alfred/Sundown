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

    void Start()
    {
        // Hitta referensen till spelets manager
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

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
        // Välj en slumpmässig riktning för clownen att röra sig åt
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector2 moveDirection = new Vector2(randomX, randomY);
        moveDirection = moveDirection.normalized; // Normalisera för jämn hastighet

        // Räkna ut spawnposition (utanför tältområdet)
        float distanceFromCenter = 1.2f;
        Vector3 centerPosition = transform.position;
        float offsetX = moveDirection.x * distanceFromCenter;
        float offsetY = moveDirection.y * distanceFromCenter;
        Vector3 spawnPosition = new Vector3(centerPosition.x + offsetX, centerPosition.y + offsetY, centerPosition.z);

        // Skapa ett nytt clownobjekt
        GameObject clownObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Ge clownen ett slumpmässigt utseende
        if (clownSprites.Length > 0)
        {
            // Välj en slumpmässig clownsprite
            int randomIndex = Random.Range(0, clownSprites.Length);
            Sprite randomClownFace = clownSprites[randomIndex];

            // Applicera på sprite renderer
            SpriteRenderer clownRenderer = clownObject.GetComponent<SpriteRenderer>();
            if (clownRenderer != null)
            {
                clownRenderer.sprite = randomClownFace;
            }
            else
            {
                Debug.LogWarning("Clown saknar SpriteRenderer!");
            }
        }

        // Ge clownen fart
        Rigidbody2D clownPhysics = clownObject.GetComponent<Rigidbody2D>();
        if (clownPhysics != null)
        {
            clownPhysics.velocity = moveDirection * objectSpeed;
        }
        else
        {
            Debug.LogWarning("Clown kan inte röra sig utan Rigidbody2D!");
        }

        // Gör spelet svårare för nästa spawn
        objectSpeed = objectSpeed + speedAcceleration;

        // Se till att farten inte blir för hög
        if (objectSpeed > 10f)
        {
            objectSpeed = 10f;
        }
    }

}