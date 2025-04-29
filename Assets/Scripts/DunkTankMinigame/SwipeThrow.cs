using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject poofPrefab;
    public GameObject ballPlaceholder;
    public Transform spawnPoint;
    public float forceMultiplier = 0.05f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwipe;
    private bool hasBallInAir = false;

    void Update()
    {
        if (hasBallInAir) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwipe = true;
            }

            if (touch.phase == TouchPhase.Ended && isSwipe)
            {
                endTouchPos = touch.position;
                Vector2 swipe = endTouchPos - startTouchPos;

                if (swipe.y > 50)
                {
                    ThrowBall(swipe);
                }

                isSwipe = false;
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            startTouchPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            Vector2 swipe = endTouchPos - startTouchPos;

            if (swipe.y > 50)
            {
                ThrowBall(swipe);
            }
        }
#endif
    }

    void ThrowBall(Vector2 swipe)
    {
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(swipe.x, swipe.y) * forceMultiplier;
        rb.AddForce(force, ForceMode2D.Impulse);

        hasBallInAir = true;

        // Hide placeholder
        if (ballPlaceholder != null)
            ballPlaceholder.SetActive(false);

        var tracker = ball.AddComponent<BallLifeTracker>();
        tracker.throwController = this;
        tracker.poofPrefab = poofPrefab;
    }

    public void BallDestroyed()
    {
        hasBallInAir = false;

        // Show placeholder again
        if (ballPlaceholder != null)
            ballPlaceholder.SetActive(true);
    }
}

