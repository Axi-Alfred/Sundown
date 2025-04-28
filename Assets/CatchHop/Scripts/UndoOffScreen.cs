using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    public float offScreenX = 15f; // Justera detta tillräckligt långt till höger
    public float offScreenY = -10f; // Justera detta om clowner faller ner också

    void Update()
    {
        // Om clownen är för långt åt höger eller för långt ner → förstör
        if (transform.position.x > offScreenX || transform.position.y < offScreenY)
        {
            Destroy(gameObject);
        }
    }

}
