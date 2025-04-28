using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetsDunk : MonoBehaviour
{
    public float speed = 2f;
    public float range = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
