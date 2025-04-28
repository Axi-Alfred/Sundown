using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager Instance;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 initialPosition;
    private float shakeTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        initialPosition = Camera.main.transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            Camera.main.transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            Camera.main.transform.localPosition = initialPosition;
        }
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}
