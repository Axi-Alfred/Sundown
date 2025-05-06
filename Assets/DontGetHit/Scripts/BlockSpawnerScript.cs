using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{
    public GameObject block;
    public float baseSpawnRate = 1.5f;
    public float widthOffset = 10f;
    public float minBlockSpacing = 1.5f;

    private float timer = 0f;
    private float currentSpawnRate;
    private int currentBlocksPerWave = 1;

    // Start kallas före första uppdateringen
    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        SkapaBlockVåg();
    }

    // Uppdateras varje frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SkapaBlockVåg();
            timer = 0f;
            
            // Uppdatera svårighetsgraden
            float nuvarandeMultiplier = DifficultyManagerScript.Instance.GetCurrentMultiplier();
            currentSpawnRate = baseSpawnRate / nuvarandeMultiplier;
            
            // Beräkna antal block per våg
            int beräknatAntalBlock = Mathf.FloorToInt(nuvarandeMultiplier / 0.5f);
            
            // Begränsa till 1-5 block
            if (beräknatAntalBlock < 1)
            {
                currentBlocksPerWave = 1;
            }
            else if (beräknatAntalBlock > 5)
            {
                currentBlocksPerWave = 5;
            }
            else
            {
                currentBlocksPerWave = beräknatAntalBlock;
            }
        }
    }

    // Skapar en våg med block
    void SkapaBlockVåg()
    {
        List<float> användaPositioner = new List<float>();
        int säkerhetsRäknare = 0;

        // Lägg till spelarens position
        användaPositioner.Add(0f);

        for (int i = 0; i < currentBlocksPerWave; i++)
        {
            bool giltigPosition = false;
            float xPos = 0f;

            while (giltigPosition == false && säkerhetsRäknare < 100)
            {
                giltigPosition = true;
                
                // Slumpa ny position
                xPos = Random.Range(
                    transform.position.x - widthOffset,
                    transform.position.x + widthOffset
                );

                // Kolla avstånd till andra positioner
                for (int j = 0; j < användaPositioner.Count; j++)
                {
                    float skillnad = Mathf.Abs(xPos - användaPositioner[j]);
                    
                    if (skillnad < minBlockSpacing)
                    {
                        giltigPosition = false;
                        break;
                    }
                }

                säkerhetsRäknare++;
            }

            användaPositioner.Add(xPos);
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }
}