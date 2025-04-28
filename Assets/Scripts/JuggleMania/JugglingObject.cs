using System.Collections;
using UnityEngine;

public class JugglingObject : MonoBehaviour
{
    public bool isKnife = false;
    public SpotController currentSpot;
    public bool isCaught = false; // IMPORTANT for fall detection
    public bool isReadyToJump = false;
    public bool isReadyToInteract = false;



    public void OnTapped()
    {
        if (!isReadyToInteract)
            return;

        if (isKnife)
        {
            Handheld.Vibrate();
            ScreenShakeManager.Instance.Shake(); // NEW: Screen shake on Knife tap
            currentSpot.KillSpot();
            Destroy(gameObject);
        }
        else
        {
            if (isReadyToJump)
            {
                isCaught = true;
                isReadyToJump = false;
                JumpToNewSpot();
            }
        }
    }


    public void JumpToNewSpot()
    {
        var availableSpots = SpotManager.Instance.GetAvailableSpots(currentSpot);
        if (availableSpots.Count == 0)
            return;

        // Choose a random other spot
        SpotController targetSpot = availableSpots[Random.Range(0, availableSpots.Count)];

        // Clear current spot
        currentSpot.currentObject = null;

        // Assign new spot
        currentSpot = targetSpot;
        currentSpot.currentObject = this;

        // Launch arc movement toward the new spot
        StartCoroutine(JumpArc(targetSpot.transform.position, 7f, 0.9f));
    }

    private IEnumerator JumpArc(Vector3 targetPosition, float arcHeight, float jumpDuration)
    {
        Vector3 startPoint = transform.position;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / jumpDuration;

            Vector3 currentPosition = Vector3.Lerp(startPoint, targetPosition, normalizedTime);
            currentPosition.y += arcHeight * Mathf.Sin(Mathf.PI * normalizedTime);

            transform.position = currentPosition;
            transform.Rotate(Vector3.forward * 360 * Time.deltaTime);

            yield return null;
        }

        transform.position = targetPosition;

        // NEW: When arc finished, object is now ready to be tapped
        transform.position = targetPosition;
        isReadyToJump = true;
        isReadyToInteract = true; // <<< NEW! Now safe to tap it

    }

    public void LaunchToSpot(Vector3 targetPosition, float arcHeight, float jumpDuration)
    {
        StartCoroutine(JumpArc(targetPosition, arcHeight, jumpDuration));
    }
}
