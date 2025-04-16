using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        IHittableByPie target = collision.GetComponent<IHittableByPie>();
        if (target != null)
        {
            target.HitByPie();
            Destroy(gameObject);
        }
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}


