using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private static Sprite backOfCard;

    [SerializeField] private TextMeshPro valueText;

    private CardShape shape;
    private int value;
    private Sprite Sprite;

    private SpriteRenderer spriteRend;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Sprite sprite, CardShape shape, int value)
    {
        this.value = value;
        this.shape = shape;
        spriteRend.sprite = sprite;
        valueText.text = value.ToString();
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
