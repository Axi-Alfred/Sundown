using UnityEngine;

public class HelmetController : MonoBehaviour
{
    public float tiltSpeed = 5f;
    public float xLimit = 7f;

    private Gyroscope gyro;
    private bool gyroEnabled;

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

        float tilt = Input.gyro.gravity.x;

        // Move based on tilt
        transform.Translate(Vector3.right * tilt * tiltSpeed * Time.deltaTime);

        // Clamp helmet within screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
