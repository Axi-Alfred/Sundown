using UnityEngine;

public class ClownWalker : MonoBehaviour
{
    public float hastighet = 2f; // Hur snabbt clownen rör sig

    // Uppdateras varje frame
    void Update()
    {
        // Flytta clownen konstant åt höger
        transform.Translate(Vector2.right * hastighet * Time.deltaTime);
    }
}