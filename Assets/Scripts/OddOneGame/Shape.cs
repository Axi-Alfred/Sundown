using UnityEngine;

public class Shape : MonoBehaviour
{
    [Header("Debug")]
    public bool isOdd = false;

    public void SetOdd(bool odd)
    {
        isOdd = odd;
        Debug.Log($"{gameObject.name} SetOdd: {odd}");
    }

    void OnMouseDown()
    {
        Debug.Log($"🖱️ Mouse clicked on: {gameObject.name}, isOdd: {isOdd}");
        HandleTap();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log($"📱 Touch tap on: {gameObject.name}, isOdd: {isOdd}");
                    HandleTap();
                }
            }
        }
    }

    void HandleTap()
    {
        FindObjectOfType<OddTapGameManager>().HandleShapeTapped(isOdd);
    }
}
