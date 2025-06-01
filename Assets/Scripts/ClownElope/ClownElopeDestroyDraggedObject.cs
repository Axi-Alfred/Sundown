using UnityEngine;

public class ClownElopeDestroyDraggedObject : MonoBehaviour
{
    // När en annan Collider träffar denna trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Draggable"))
        {
            ClownElopeDragToCenter dragScript = other.GetComponent<ClownElopeDragToCenter>();
            if (dragScript != null && dragScript.isDragging)
            {
                Debug.Log("Förstördes");
                Destroy(other.gameObject);
            }
        }
    }
}