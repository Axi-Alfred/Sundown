using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float verticalSpeed;
    public float horizontalAmplitude = 0.5f;
    public float horizontalFrequency = 1f;

    public GameObject popEffect;
    public AudioClip popSound;

    private float initialX;

    void Start()
    {
        initialX = transform.position.x;
        horizontalFrequency = Random.Range(0.5f, 1.5f);
        horizontalAmplitude = Random.Range(0.3f, 0.8f);
    }

    void Update()
    {
        transform.Translate(Vector2.up * verticalSpeed * Time.deltaTime);

        float newX = initialX + Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (transform.position.y > 6f)
            Destroy(gameObject);
    }

    void OnMouseDown()
    {
        PopBalloon();
    }

    void PopBalloon()
    {
        if (popEffect != null)
            Instantiate(popEffect, transform.position, Quaternion.identity);

        if (popSound != null)
            AudioSource.PlayClipAtPoint(popSound, Camera.main.transform.position);

        FindObjectOfType<BalloonGameManager>().BalloonPopped();
        Destroy(gameObject);
    }
}
