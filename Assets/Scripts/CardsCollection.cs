using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsCollection : MonoBehaviour
{
    public CardData[] cardsCollection = new CardData[3]; 
    public Sprite[] cardSprites = new Sprite[3];

    [SerializeField] private GameObject cardPrefab;

    void Start()
    {
        CreateCard(cardSprites[0], CardShape.Circle, 5, 0);
        CreateCard(cardSprites[1], CardShape.Square, 10, 1);
        CreateCard(cardSprites[2], CardShape.Triangle, 3, 2);
    }

    private void CreateCard(Sprite sprite, CardShape shape, int value, int index)
    {
        cardsCollection[index] = new CardData(sprite, shape, value);
    }

    public void SpawnCard(int index)
    {
        if (cardsCollection[index] != null)
        {
            GameObject newCard = Instantiate(cardPrefab);
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.Initialize(cardsCollection[index]); 
        }
    }
}
