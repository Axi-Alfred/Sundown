using UnityEngine;

public class TrampolineDrag : MonoBehaviour
{
    void Update()
    {
        // Hantera touch-input
        if (Input.touchCount > 0)
        {
            Touch pek = Input.GetTouch(0);
            HanteraInput(pek.position);
        }
        // Hantera mus-input i editorn
        else if (Application.isEditor && Input.GetMouseButton(0))
        {
            HanteraInput(Input.mousePosition);
        }
    }

    void HanteraInput(Vector2 skärmPosition)
    {
        // Omvandla till spelvärldens position
        Vector3 världsPosition = Camera.main.ScreenToWorldPoint(skärmPosition);
        världsPosition.z = 0;

        // Flytta trampolinen i X-led
        transform.position = new Vector3(världsPosition.x, transform.position.y, 0);
    }
}