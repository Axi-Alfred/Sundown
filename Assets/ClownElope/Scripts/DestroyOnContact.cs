using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private ClownElopeManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<ClownElopeManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object escaped and hit the wall!");
        gameManager.ObjectEscaped(); // Count the escape
        Destroy(other.gameObject);
    }
}
