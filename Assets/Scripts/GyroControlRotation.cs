using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControlRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (GyroWarning.IsCalibrated())
        {
            {
                float zRotation = Input.gyro.rotationRateUnbiased.z;
                transform.Rotate(0, 0, -zRotation * rotationSpeed * Time.deltaTime);
            }
        }

    }
}