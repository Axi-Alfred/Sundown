using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    public void HitByPie()
    {
        // Play animation or sound here if you want
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pie"))
        {
            HitByPie();
            Destroy(collision.gameObject); // Remove the pie
        }
    }
}

