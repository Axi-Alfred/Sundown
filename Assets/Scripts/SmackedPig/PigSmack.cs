using UnityEngine;
using UnityEngine.EventSystems;

public class PigSmack : MonoBehaviour, IPointerDownHandler
{
    public Sprite normalPig;
    public Sprite smackedPig;

    private SpriteRenderer spriteRenderer;
    private bool hasBeenSmacked = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalPig;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!hasBeenSmacked)
        {
            hasBeenSmacked = true;
            spriteRenderer.sprite = smackedPig;

            // ✅ Award point
            PlayerData.currentPlayerTurn.AddScore(1);

            // ✅ End the round
            GameManager1.EndTurn();
        }
    }
}
