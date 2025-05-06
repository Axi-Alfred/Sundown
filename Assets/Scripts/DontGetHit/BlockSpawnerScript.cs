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

    // Start kallas f�re f�rsta uppdateringen
    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        SkapaBlockV�g();
    }

    // Uppdateras varje frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SkapaBlockV�g();
            timer = 0f;

            // Uppdatera sv�righetsgraden
            float nuvarandeMultiplier = DifficultyManagerScript.Instance.GetCurrentMultiplier();
            currentSpawnRate = baseSpawnRate / nuvarandeMultiplier;

            // Ber�kna antal block per v�g
            int ber�knatAntalBlock = Mathf.FloorToInt(nuvarandeMultiplier / 0.5f);

            // Begr�nsa till 1-5 block
            if (ber�knatAntalBlock < 1)
            {
                currentBlocksPerWave = 1;
            }
            else if (ber�knatAntalBlock > 5)
            {
                currentBlocksPerWave = 5;
            }
            else
            {
                currentBlocksPerWave = ber�knatAntalBlock;
            }
        }
    }

    // Skapar en v�g med block
    void SkapaBlockV�g()
    {
        List<float> anv�ndaPositioner = new List<float>();
        int s�kerhetsR�knare = 0;

        // L�gg till spelarens position
        anv�ndaPositioner.Add(0f);

        for (int i = 0; i < currentBlocksPerWave; i++)
        {
            bool giltigPosition = false;
            float xPos = 0f;

            while (giltigPosition == false && s�kerhetsR�knare < 100)
            {
                giltigPosition = true;

                // Slumpa ny position
                xPos = Random.Range(
                    transform.position.x - widthOffset,
                    transform.position.x + widthOffset
                );

                // Kolla avst�nd till andra positioner
                for (int j = 0; j < anv�ndaPositioner.Count; j++)
                {
                    float skillnad = Mathf.Abs(xPos - anv�ndaPositioner[j]);

                    if (skillnad < minBlockSpacing)
                    {
                        giltigPosition = false;
                        break;
                    }
                }

                s�kerhetsR�knare++;
            }

            anv�ndaPositioner.Add(xPos);
            Instantiate(block, new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
        }
    }
}