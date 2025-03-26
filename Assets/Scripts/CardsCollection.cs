using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsCollection : MonoBehaviour
{
    public GameObject[] cardsCollection = new GameObject[3];
    public Sprite[] cardSprites = new Sprite[3];

    [SerializeField] private GameObject cardsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateCard(cardSprites[2], CardShape.Shape2, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCard(Sprite sprite, CardShape shape, int value, int cardIndexInArray)
    {
        GameObject newCard = Instantiate(cardsPrefab);
        Card cardClass = newCard.GetComponent<Card>(); 
        cardClass.Initialize(sprite, shape, value);
        cardsCollection[cardIndexInArray] = newCard;
    }
}
