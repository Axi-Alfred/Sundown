using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kolla om det är bollen
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Bollen gick i hålet! 🎉");

            // Försvinn bollen
            Destroy(other.gameObject);
        }
    }
}
