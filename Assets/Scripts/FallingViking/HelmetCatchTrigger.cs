using UnityEngine;

public class HelmetCatchTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Viking"))
        {
            Destroy(other.gameObject);
            FallingVikingGameManager.Instance.OnVikingCaught();
        }
        else if (other.CompareTag("Axe"))
        {
            Destroy(other.gameObject);
            FallingVikingGameManager.Instance.OnAxeCaught();
        }
    }
}
