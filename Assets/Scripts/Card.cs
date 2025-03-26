using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private static Sprite backOfCard;

    [SerializeField] private TextMeshPro valueText;

    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void Initialize(CardData cardData)
    {
        spriteRend.sprite = cardData.sprite;
        valueText.text = cardData.value.ToString();
    }
}
