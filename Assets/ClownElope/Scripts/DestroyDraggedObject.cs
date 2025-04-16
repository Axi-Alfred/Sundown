using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDraggedObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Draggable"))
        {
            DragToCenter dragScript = other.GetComponent<DragToCenter>();

            if (dragScript != null && dragScript.isDragging)
            {
                Debug.Log("Destroyed");
                Destroy(other.gameObject);
            }
        }
    }

}
