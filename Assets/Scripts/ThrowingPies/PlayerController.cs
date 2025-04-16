using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float swipeSpeedMultiplier = 0.05f;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private float swipeStartTime;
    public float targetWorldHeight = 2f; // how tall (in world units) the hand should be visually

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
        {
            Debug.LogWarning("Hand has no SpriteRenderer or sprite assigned.");
            return;
        }

        float spriteHeight = sr.sprite.bounds.size.y;
        float scaleFactor = targetWorldHeight / spriteHeight;

        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        SimulateSwipeWithMouse();
#else
        DetectSwipeWithTouch();
#endif

        // Optional: still support keyboard
        float input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(input) > 0.1f)
        {
            transform.position += new Vector3(input * 5f * Time.deltaTime, 0f, 0f);
        }
    }
    void LateUpdate()
    {
        float verticalSize = Camera.main.orthographicSize;
        float horizontalSize = verticalSize * Screen.width / Screen.height;

        Vector3 pos = transform.position;

        // Clamp horizontal (X) to screen bounds
        pos.x = Mathf.Clamp(pos.x, -horizontalSize + 0.5f, horizontalSize - 0.5f);

        // Lock Y to bottom of screen
        pos.y = -verticalSize + (targetWorldHeight / 2f) + 0.1f;

        transform.position = pos;
    }


    void DetectSwipeWithTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                swipeStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                float swipeDuration = Time.time - swipeStartTime;
                float swipeDistance = endTouchPos.x - startTouchPos.x;
                float swipeSpeed = swipeDistance / swipeDuration;
                float moveAmount = swipeSpeed * swipeSpeedMultiplier;
                transform.position += new Vector3(moveAmount, 0f, 0f);
            }
        }
    }

    void SimulateSwipeWithMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            swipeStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            float swipeDuration = Time.time - swipeStartTime;
            float swipeDistance = endTouchPos.x - startTouchPos.x;
            float swipeSpeed = swipeDistance / swipeDuration;
            float moveAmount = swipeSpeed * swipeSpeedMultiplier;
            transform.position += new Vector3(moveAmount, 0f, 0f);
        }
    }


}






