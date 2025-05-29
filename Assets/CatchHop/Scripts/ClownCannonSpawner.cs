using UnityEngine;

public class ClownCannonSpawner : MonoBehaviour
{
    public GameObject[] clownPrefabs;       // Lista med olika clown-prefabs
    public Transform spawnPosition;         // Position där clowner skapas
    public Animator cannonAnimator;         // Animator för kanonen

    public float spawnInterval = 2f;        // Tid mellan varje clown (i sekunder)
    public float minInterval = 0.3f;        // Minsta tillåtna intervall
    public float difficultyStep = 0.05f;    // Hur mycket intervallet minskar varje gång

    public float minForce = 5f;             // Minsta kraft för att skjuta clownen
    public float maxForce = 15f;            // Största kraft för att skjuta clownen
    public float minAngle = -30f;           // Minsta vinkel i grader
    public float maxAngle = 30f;            // Största vinkel i grader

    private float timer;                    // Timer för att hålla koll på när nästa clown ska skapas

    void Start()
    {
        // Sätt timern till startvärdet
        timer = spawnInterval;

        // Skapa en clown direkt när spelet startar
        SpawnClown();
    }

    void Update()
    {
        // Minska timern med den tid som gått sedan förra bildrutan
        timer = timer - Time.deltaTime;

        // Om timern har gått ut, skapa en ny clown
        if (timer <= 0f)
        {
            SpawnClown();

            // Minska intervallet för att öka svårighetsgraden, men inte under minsta tillåtna intervall
            if (spawnInterval - difficultyStep > minInterval)
            {
                spawnInterval = spawnInterval - difficultyStep;
            }
            else
            {
                spawnInterval = minInterval;
            }

            // Starta om timern
            timer = spawnInterval;
        }
    }

    void SpawnClown()
    {
        // Spela upp kanonens squish-animation om Animator är tilldelad
        if (cannonAnimator != null)
        {
            cannonAnimator.Play("KanonSquish", -1, 0f);
        }

        // Välj en slumpmässig clown-prefab från listan
        int totalClowns = clownPrefabs.Length;
        int index = Random.Range(0, totalClowns);
        GameObject selectedPrefab = clownPrefabs[index];

        // Skapa clownen vid spawnPosition
        Vector3 spawnPos = spawnPosition.position;
        Quaternion rotation = Quaternion.identity;
        GameObject newClown = Instantiate(selectedPrefab, spawnPos, rotation);

        // Hämta clownens Rigidbody2D-komponent
        Rigidbody2D rb = newClown.GetComponent<Rigidbody2D>();

        // Om clownen har en Rigidbody2D-komponent
        if (rb != null)
        {
            // Slumpa en kraft mellan minsta och största värde
            float force = Random.Range(minForce, maxForce);

            // Slumpa en vinkel mellan minsta och största värde
            float angle = Random.Range(minAngle, maxAngle);

            // Omvandla vinkeln till radianer
            float angleInRadians = angle * Mathf.Deg2Rad;

            // Räkna ut riktningen som clownen ska kastas i
            float x = Mathf.Cos(angleInRadians);
            float y = Mathf.Sin(angleInRadians);
            Vector2 direction = new Vector2(x, y);

            // Ge clownen en kraft i den riktningen
            Vector2 forceVector = direction * force;
            rb.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}