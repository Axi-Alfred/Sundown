using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D playerRidgidbody;

    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        TouchmoveSpeed();
    }

    void TouchmoveSpeed()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touchPosition.x < 0)
            {
                //moveSpeed left
                playerRidgidbody.velocity = Vector2.left * moveSpeed;
            }
            else
            {
                //moveSpeed right
                playerRidgidbody.velocity = Vector2.right * moveSpeed;
            }
        }
        else
        {
            playerRidgidbody.velocity = Vector2.zero;
        }
    }
}
