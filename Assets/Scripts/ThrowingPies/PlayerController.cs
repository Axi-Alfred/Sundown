using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 currentPos = transform.position;

        // Move only on the X axis
        transform.position = new Vector3(
            currentPos.x + moveInput * moveSpeed * Time.deltaTime,
            currentPos.y,
            currentPos.z
        );
    }
}



