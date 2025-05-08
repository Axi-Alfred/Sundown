using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PointerSpring : MonoBehaviour
{
    public float returnSpeed = 8f;
    public float maxAngle = 45f;

    private Rigidbody2D rb;
    private Vector3 startEulerAngles;
    private Vector3 pointerPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startEulerAngles = transform.localEulerAngles;
        pointerPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        // Get current rotation angle
        float currentAngle = transform.localEulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;

        // Calculate return force
        float targetAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);
        float angleDifference = -targetAngle * returnSpeed;

        // Apply rotation
        rb.MoveRotation(Mathf.LerpAngle(currentAngle, 0, returnSpeed * Time.fixedDeltaTime));

    }

    private void LateUpdate()
    {
        transform.localPosition = pointerPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Briefly disable physics to prevent wheel interference
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
