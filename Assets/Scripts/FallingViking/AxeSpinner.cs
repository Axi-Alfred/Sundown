using UnityEngine;

public class AxeSpinner : MonoBehaviour
{
    public float spinSpeed = 250f;

    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
