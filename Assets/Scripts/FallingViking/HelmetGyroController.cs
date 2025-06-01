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
        Vector3 newPosition = transform.position + new Vector3(tilt * speed * Time.deltaTime, 0, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -7f, 7f);
        newPosition.y = transform.position.y;  // lock Y
        newPosition.z = transform.position.z;  // lock Z (if needed)

        transform.position = newPosition;
    }
}
