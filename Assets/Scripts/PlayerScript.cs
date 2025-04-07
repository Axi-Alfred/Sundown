using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float xBoundary = 8f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleTouchInput();
        ClampPosition();
    }

    void HandleTouchInput()
    {
        if (!Input.GetMouseButton(0))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.velocity = touchPosition.x < 0 ?
            Vector2.left * moveSpeed :
            Vector2.right * moveSpeed;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xBoundary, xBoundary);
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Game Over");
            // Add your game over logic here
            Time.timeScale = 0; // Freeze game
        }
    }
}
