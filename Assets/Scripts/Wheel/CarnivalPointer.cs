using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSpring : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 8f;
    [SerializeField] private float maxAngle = 45f;

    [SerializeField] private AudioClip pinAudio;

    private Rigidbody2D rb;
    private Vector3 startEulerAngles;
    private Vector3 pointerPosition;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startEulerAngles = transform.localEulerAngles;
        pointerPosition = transform.localPosition;

        audioSource = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {
        float currentAngle = transform.localEulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;

        float targetAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);
        float angleDifference = -targetAngle * returnSpeed;

        rb.MoveRotation(Mathf.LerpAngle(currentAngle, 0, returnSpeed * Time.fixedDeltaTime));

    }

    private void LateUpdate()
    {
        transform.localPosition = pointerPosition;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        print("zuzu");

        audioSource.clip = pinAudio;
        audioSource.Play();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
