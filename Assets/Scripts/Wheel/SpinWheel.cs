using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private float wheelMotionlessThreshold = 0.5f;
    [SerializeField] private Pointer pointer;

    private Rigidbody2D rb2D;
    private Vector2 lastTouchPos;
    private bool isDragging;
    private bool isSpinning;
    private bool hasSpinned; //has already bombaclat spinned and is currently stationary
    private bool hasReachedMotionThreshold;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float wheelVelocity = Mathf.Abs(rb2D.angularVelocity);


        if (isSpinning)
        {
            if (!hasReachedMotionThreshold && wheelVelocity > wheelMotionlessThreshold)
            {
                hasReachedMotionThreshold = true;
            }

            if (hasReachedMotionThreshold && wheelVelocity < wheelMotionlessThreshold)
            {
                wheelVelocity = 0;
                hasSpinned = true;
                isSpinning = false;
                hasReachedMotionThreshold = false;
            }
        }

        if (Input.touchCount > 0 && !isSpinning && !hasSpinned)
        {
            SpinTheWheel();
        }

        pointer.WheelHasSpinned(hasSpinned);

    }

    private void SpinTheWheel()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isDragging = true;
            lastTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 delta = touch.position - lastTouchPos;
            float spinForce = delta.x * spinSpeed;
            if (Mathf.Abs(spinForce) > 0.1)
            {
                rb2D.AddTorque(spinForce);
                isSpinning = true;
            }

            lastTouchPos = touch.position;

        }
        else if (touch.phase == TouchPhase.Ended)
        {
            isDragging = false;
        }
    }

    public void ResetWheel()
    {
        rb2D.angularVelocity = 0;
        GetComponent<Rigidbody2D>().rotation = 0f;
        hasSpinned = false;
        isSpinning = false;
        hasReachedMotionThreshold = false;
    }
}
