using UnityEngine;

public class HelmetCatchTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Viking"))
        {
            Destroy(other.gameObject);
            SFX.Play(1);
            FallingVikingGameManager.Instance.OnVikingCaught();
        }
        else if (other.CompareTag("Axe"))
        {
            Destroy(other.gameObject);
            SFX.Play(2);
            FallingVikingGameManager.Instance.OnAxeCaught();
        }
    }
}
