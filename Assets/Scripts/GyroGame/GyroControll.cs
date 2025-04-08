using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float zRotation = Input.gyro.rotationRateUnbiased.z;
        transform.Translate(-zRotation * movementSpeed * Time.deltaTime, 0, 0);
    }
}