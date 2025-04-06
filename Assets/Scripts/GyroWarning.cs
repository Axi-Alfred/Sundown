using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroWarning : MonoBehaviour
{
    public bool isCalibrated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCalibrated)
        {
            Vector3 gravity = Input.gyro.gravity;

            // Acceptable margin of error
            float tolerance = 0.2f;

            // Check if the phone is being held "straight"
            if (Mathf.Abs(gravity.x) > tolerance || Mathf.Abs(gravity.z) > tolerance)
            {
                print("Please hold the phone straight before starting!");
            }
            else
            {
                print("sPhone is held straight. Starting...");
                isCalibrated = true;

                // Call your game-start method here
                // StartGame();
            }
        }
    }
}
