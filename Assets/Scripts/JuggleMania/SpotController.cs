using System.Collections;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    public bool isAlive = true;
    public JugglingObject currentObject;
    public HandButton linkedButton;

    public SFXLibrary sfxLibrary;

    public void OnTapped()
    {
        if (!isAlive || currentObject == null)
            return;

        currentObject.OnTapped();
        sfxLibrary.Play(1);
    }

    public void KillSpot()
    {
        if (!isAlive)
            return;

        isAlive = false;

        if (linkedButton != null)
        {
            linkedButton.SetDeadVisual();
        }

        SpotManager.Instance.deadHands++; // NEW: Increment counter

        FindObjectOfType<BackgroundManager>().UpdateBackground(SpotManager.Instance.deadHands);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only react if something is supposed to land here
        if (currentObject != null && collision.gameObject == currentObject.gameObject)
        {
            // Object reached the spot!
            if (!currentObject.isCaught)
            {
                StartCoroutine(DestroyIfIgnored(currentObject));
            }
        }
    }

    private IEnumerator DestroyIfIgnored(JugglingObject obj)
    {
        yield return new WaitForSeconds(2f); // Give the player some reaction time (adjust if you want)

        if (!obj.isCaught)
        {
            Destroy(obj.gameObject);
            currentObject = null;
        }
    }
}
