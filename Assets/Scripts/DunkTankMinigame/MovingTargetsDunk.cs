using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetsDunk : MonoBehaviour
{
    public float speed = 2f; // How fast the targets move sideways
    public float range = 2f; // How far from the spawnpoint the targets can move
    private Vector3 startPos; // Stores the original position 

    // Called once at the beginning when the targets are created
    void Start()
    {
        // Save the initial position of the object
        startPos = transform.position;
    }

    // Called once per frame
    void Update()
    {
        // Calculate the horizontal offset using a sine wave
        float offset = Mathf.Sin(Time.time * speed) * range;

        // Apply the offset to the x psotion, keeping y and z unchanged
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
