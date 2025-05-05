using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickablePie : MonoBehaviour, IPointerClickHandler
{
    private bool isOdd;
    private PiePickerGameManager manager;

    public void Setup(Sprite sprite, bool isOddPie, PiePickerGameManager gameManager)
    {
        GetComponent<Image>().sprite = sprite;
        isOdd = isOddPie;
        manager = gameManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.HandlePieTapped(isOdd);
    }
}
