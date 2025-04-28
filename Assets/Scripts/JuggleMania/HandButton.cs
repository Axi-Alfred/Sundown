using UnityEngine;
using UnityEngine.UI;

public class HandButton : MonoBehaviour
{
    public SpotController linkedSpot; // Optional, if you want
    public Sprite normalSprite; // Normal hand sprite
    public Sprite deadSprite;   // Dead hand sprite

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnHandPressed()
    {
        if (linkedSpot != null)
        {
            linkedSpot.OnTapped();
        }
    }

    public void SetDeadVisual()
    {
        if (buttonImage != null && deadSprite != null)
        {
            buttonImage.sprite = deadSprite;
        }
    }
}
