using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Rör sig i Z
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Krymper över tid
        float shrinkRate = 0.1f * Time.deltaTime;
        transform.localScale -= new Vector3(shrinkRate, shrinkRate, 0);

        // Förstör kulan om den blir för liten
        if (transform.localScale.x <= 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
