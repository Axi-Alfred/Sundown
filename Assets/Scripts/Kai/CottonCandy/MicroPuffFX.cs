using System.Collections;
using UnityEngine;

public class MicroPuffFX : MonoBehaviour
{
    public float lifetime = 0.4f;

    void Start()
    {
        StartCoroutine(PuffRoutine());
    }

    IEnumerator PuffRoutine()
    {
        float t = 0f;
        float scaleDuration = 0.2f;
        Vector3 finalScale = transform.localScale;

        transform.localScale = Vector3.zero;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.SmoothStep(0, 1, t / scaleDuration);
            transform.localScale = finalScale * s;
            yield return null;
        }

        Destroy(gameObject, lifetime);
    }
}
