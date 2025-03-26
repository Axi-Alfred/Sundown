using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private static Sprite backOfCard;
    private CardShape shape;
    private int value;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    private void Initialize()
    {

    }

    private void TestMethod()
    {
        print("Card");
    }
}

public enum CardShape
{
    Shape1, Shape2, Shape3, Shape4
}
