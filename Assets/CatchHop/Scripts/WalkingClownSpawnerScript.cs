using UnityEngine;

public class WalkingClownSpawnerScript : MonoBehaviour
{
    public GameObject[] clownPrefabs;       // Lista med olika clown-prefabs
    public Transform spawnPosition;         // Position där clowner skapas
    public Animator kanonAnimator;          // Animator för kanonen

    public float spawnIntervall = 2f;       // Tid mellan varje clown (i sekunder)
    public float minstaIntervall = 0.3f;    // Minsta tillåtna intervall
    public float svårighetsTak = 0.05f;     // Hur mycket intervallet minskar varje gång

    public float minKraft = 5f;             // Minsta kraft för att skjuta clownen
    public float maxKraft = 15f;            // Största kraft för att skjuta clownen
    public float minVinkel = -30f;          // Minsta vinkel i grader
    public float maxVinkel = 30f;           // Största vinkel i grader

    private float timer;                    // Timer för att hålla koll på när nästa clown ska skapas

    void Start()
    {
        // Sätt timern till startvärdet
        timer = spawnIntervall;

        // Skapa en clown direkt när spelet startar
        SkapaClown();
    }

    void Update()
    {
        // Minska timern med den tid som gått sedan förra bildrutan
        timer = timer - Time.deltaTime;

        // Om timern har gått ut, skapa en ny clown
        if (timer <= 0f)
        {
            SkapaClown();

            // Minska intervallet för att öka svårighetsgraden, men inte under minsta tillåtna intervall
            if (spawnIntervall - svårighetsTak > minstaIntervall)
            {
                spawnIntervall = spawnIntervall - svårighetsTak;
            }
            else
            {
                spawnIntervall = minstaIntervall;
            }

            // Starta om timern
            timer = spawnIntervall;
        }
    }

    void SkapaClown()
    {
        // Spela upp kanonens squish-animation om Animator är tilldelad
        if (kanonAnimator != null)
        {
            kanonAnimator.Play("KanonSquish", -1, 0f);
        }

        // Välj en slumpmässig clown-prefab från listan
        int antalClowner = clownPrefabs.Length;
        int index = Random.Range(0, antalClowner);
        GameObject valdClownPrefab = clownPrefabs[index];

        // Skapa clownen vid spawnPosition
        Vector3 spawnPos = spawnPosition.position;
        Quaternion rotation = Quaternion.identity;
        GameObject nyClown = Instantiate(valdClownPrefab, spawnPos, rotation);

        // Hämta clownens Rigidbody2D-komponent
        Rigidbody2D rb = nyClown.GetComponent<Rigidbody2D>();

        // Om clownen har en Rigidbody2D-komponent
        if (rb != null)
        {
            // Slumpa en kraft mellan minsta och största värde
            float kraft = Random.Range(minKraft, maxKraft);

            // Slumpa en vinkel mellan minsta och största värde
            float vinkel = Random.Range(minVinkel, maxVinkel);

            // Omvandla vinkeln till radianer
            float vinkelIRadianer = vinkel * Mathf.Deg2Rad;

            // Räkna ut riktningen som clownen ska kastas i
            float x = Mathf.Cos(vinkelIRadianer);
            float y = Mathf.Sin(vinkelIRadianer);
            Vector2 riktning = new Vector2(x, y);

            // Ge clownen en kraft i den riktningen
            Vector2 kraftVektor = riktning * kraft;
            rb.AddForce(kraftVektor, ForceMode2D.Impulse);
        }
    }
}