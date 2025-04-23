using UnityEngine;

public class HelmetGyroController : MonoBehaviour
{
    public float speed = 5f;
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

        float tilt = gyro.gravity.x;
        Vector3 movement = new Vector3(tilt * speed, 0, 0);
        transform.Translate(movement * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -7f, 7f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
