using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CardsCollection cardsCollection;

    // Start is called before the first frame update
    void Start()
    {
        cardsCollection = GetComponent<CardsCollection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cardsCollection.SpawnCard(Random.Range(0, cardsCollection.cardsCollection.Length-1));
        }
    }
}
