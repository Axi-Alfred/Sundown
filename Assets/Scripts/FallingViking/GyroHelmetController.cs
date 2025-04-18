using UnityEngine;

public class GyroHelmetController : MonoBehaviour
{
    public float tiltSpeed = 5f;
    private bool gyroEnabled;
    private Gyroscope gyro;

    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    void Update()
    {
        if (!gyroEnabled) return;

        // Get the tilt along the x-axis of the phone
        float tilt = Input.gyro.gravity.x;

        // Move the helmet left/right based on tilt
        transform.Translate(Vector3.right * tilt * tiltSpeed * Time.deltaTime);
    }
}
