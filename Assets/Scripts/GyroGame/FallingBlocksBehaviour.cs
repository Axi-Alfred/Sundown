using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlocksBehaviour : MonoBehaviour
{
    [SerializeField] private float fallSpeed;

    private bool isFalling;

    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        isFalling = true;
        rb2D.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFalling)
        {
            rb2D.velocity = new Vector2(0, -fallSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GyroGameBlock") || other.gameObject.CompareTag("Root"))
        {
            isFalling = false;
            rb2D.gravityScale = 1.5f;

            GameObject root = GameObject.FindGameObjectWithTag("Root");
            transform.SetParent(root.transform);
        }

        if (other.gameObject.CompareTag("GyroGameGround"))
        {
            Destroy(gameObject);
        }
    }
}
