using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float xBoundary = 17.8f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Automatically set boundaries based on camera view
        float screenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        xBoundary = screenWidth - 1f; // 1f padding
    }

    void Update()
    {
        // Draw left boundary
        Debug.DrawLine(
            new Vector3(-xBoundary, -10),  // Start point (left bottom)
            new Vector3(-xBoundary, 10),   // End point (left top)
            Color.red                      // Color: RED
        );

        // Draw right boundary
        Debug.DrawLine(
            new Vector3(xBoundary, -10),   // Start point (right bottom)
            new Vector3(xBoundary, 10),    // End point (right top)
            Color.red                      // Color: RED
        );
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
            // Game logic here
            gameObject.SetActive(false);
        }
    }
}
