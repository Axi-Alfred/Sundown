using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeThrow : MonoBehaviour
{
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwipe = false;

    public GameObject ballPrefab;
    public Transform throwPoint;
    public float throwForceMultiplier = 5f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPos = touch.position;
                    isSwipe = true;
                    break;

                case TouchPhase.Ended:
                    if (isSwipe)
                    {
                        endTouchPos = touch.position;
                        Vector2 swipeVector = endTouchPos - startTouchPos;

                        if (swipeVector.y > 50) // Threshold to prevent accidental throws
                        {
                            ThrowBall(swipeVector);
                        }
                    }
                    isSwipe = false;
                    break;
            }
        }
    }

    void ThrowBall(Vector2 swipeVector)
    {
        Vector3 direction = new Vector3(swipeVector.x, swipeVector.y, 0);
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(direction.x, direction.y, 10f) * throwForceMultiplier); // z for forward
    }
}

