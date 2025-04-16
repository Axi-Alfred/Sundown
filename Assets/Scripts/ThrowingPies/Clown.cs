using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Clown : MonoBehaviour
{
    public void HitByPie()
    {
        // Optional: play sound, animation, effects here
        Debug.Log("Clown got pied!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pie>())
        {
            HitByPie();
            Destroy(collision.gameObject); // Destroy the pie too
        }
    }
}


