using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pie : MonoBehaviour
{
    private float speed;

    public void Launch(float spd)
    {
        speed = spd;
    }

    void Update()
    {
        // Move straight up (Y-axis)
        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Clown>())
        {
            collision.GetComponent<Clown>().HitByPie();
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}


