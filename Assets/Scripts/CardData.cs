using UnityEngine;

public enum CardShape
{
    Circle, Square, Triangle
}

public class CardData
{
    public Sprite sprite;
    public CardShape shape;
    public int value;

    public CardData(Sprite sprite, CardShape shape, int value)
    {
        this.sprite = sprite;
        this.shape = shape;
        this.value = value;
    }
}
