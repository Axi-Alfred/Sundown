using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControlMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 50f;

    private Rigidbody2D rb2D;
    private float horizontalValue;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            print("This device does not support this game!");
        }

        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float zRotation = Input.gyro.rotationRateUnbiased.z;
        //print(zRotation);
        horizontalValue = zRotation * movementSpeed;
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(-horizontalValue, rb2D.velocity.y);
    }

    //Testa Viggos ide om du har tid med att skära dem
}