using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour, IHittableByPie
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void HitByPie()
    {
        Debug.Log("Clown got pied!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Clown touched by player!");
            Destroy(gameObject);
        }
    }
}






