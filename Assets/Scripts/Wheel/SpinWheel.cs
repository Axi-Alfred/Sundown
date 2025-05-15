using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private float wheelMotionlessThreshold = 0.5f;
    [SerializeField] private float torqueMultiplier = 50f;
    [SerializeField] private Pointer pointer;
    [SerializeField] private GameObject pointerObject; //needed to check that the rotation of the pointer is 0 before starting a game

    private Rigidbody2D rb2D;
    private Vector2 lastTouchPos;
    private Vector2 wheelCenter;
    private bool isDragging;
    private bool isSpinning;
    private bool hasSpinned; //has already bombaclat spinned and is currently stationary
    private bool hasReachedMotionThreshold;

    [SerializeField] private float minDragDistance;
    [SerializeField] private float minSpinForce;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        wheelCenter = Camera.main.WorldToScreenPoint(transform.position);
        float wheelRotation = UnityEngine.Random.Range(1, 90);
        transform.localRotation = Quaternion.Euler(0, 0, wheelRotation);
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

            //The wheel has/will come to a stop 
            if (hasReachedMotionThreshold && wheelVelocity < wheelMotionlessThreshold && Mathf.Abs(pointerObject.transform.eulerAngles.z) < 5)
            {
                rb2D.angularVelocity = 0;
                hasSpinned = true;
                isSpinning = false;
                hasReachedMotionThreshold = false;
                Handheld.Vibrate();
            }
        }

        if (Input.touchCount > 0 && !isSpinning && !hasSpinned)
        {
            SpinTheWheel(Input.GetTouch(0));
        }

        pointer.WheelHasSpinned(hasSpinned);
    }

    private void SpinTheWheel(Touch touch)
    {
        if (hasSpinned || isSpinning) return;


        if (touch.phase == TouchPhase.Began)
        {
            isDragging = true;
            lastTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 currentTouchPos = touch.position;
            float dragDistance = Vector2.Distance(currentTouchPos, lastTouchPos);

            if (dragDistance < minDragDistance) return;

            Vector2 from = lastTouchPos - wheelCenter;
            Vector2 to = currentTouchPos - wheelCenter;

            float angle = Vector2.SignedAngle(from, to);

            float touchVelocity = touch.deltaPosition.magnitude / touch.deltaTime;

            float torque = Mathf.Max(minSpinForce, Mathf.Abs(angle * touchVelocity * torqueMultiplier));

            rb2D.AddTorque(-angle * -torque);

            lastTouchPos = currentTouchPos;
            isSpinning = true;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;
        }
    }

    public void SpinWithButton()
    {
        int randomSpinForce = UnityEngine.Random.Range(500, 1000);
        rb2D.AddTorque(-randomSpinForce);
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
