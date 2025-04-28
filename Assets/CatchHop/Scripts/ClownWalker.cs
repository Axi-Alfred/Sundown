using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownWalker : MonoBehaviour
{
    public float speed = 2f; // 👈 Gårhastighet (kan ändras i inspectorn)

    void Update()
    {
        // 👉 Flytta clownen åt höger varje frame
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
