using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkWithHeight : MonoBehaviour
{
    private Vector3 startScale;
    private float minScale = 0.3f; // Min size of the ball
    private float maxHeight = 10f; // How far up before fully shrunk
    private float startY;

    void Start()
    {
        startScale = transform.localScale;
        startY = transform.position.y;
    }

    void Update()
    {
        // Räkna ut hur långt bollen har fallit nedåt från sin startposition
        float heightTravelled = startY - transform.position.y;

        // Beräkna hur mycket den ska krympa
        float scaleFactor = 1 - Mathf.Clamp01(heightTravelled / maxHeight);

        // Sätt en minsta storlek så bollen inte försvinner helt
        float clampedScale = Mathf.Max(scaleFactor, minScale);

        // Uppdatera bollens skala
        transform.localScale = startScale * clampedScale;
    }
}
