using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pie : MonoBehaviour
{
    private float direction;
    private float speed;

    public void Launch(float dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    void Update()
    {
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Clown>())
        {
            collision.GetComponent<Clown>().HitByPie();
            Destroy(gameObject);
        }
    }
}

