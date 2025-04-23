using UnityEngine;

public class HelmetCatchTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Viking"))
        {
            Destroy(other.gameObject);
            FallingVikingGameManager.Instance.AddScore(1);
        }
        else if (other.CompareTag("Axe"))
        {
            Destroy(other.gameObject);
            FallingVikingGameManager.Instance.AddScore(-2);
        }
    }
}
