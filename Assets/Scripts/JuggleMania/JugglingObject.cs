using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class JugglingObject : MonoBehaviour, IPointerDownHandler
{
    public bool isKnife = false;
    public SpotController currentSpot;
    public bool isCaught = false;
    public bool isReadyToJump = false;
    public bool isReadyToInteract = false;



    public void OnTapped()
    {
        if (!isReadyToInteract)
            return;

        if (isKnife)
        {
            SFX.Play(2);
#if UNITY_ANDROID || UNITY_IOS // Kompilera endast på mobiler
Handheld.Vibrate();
#endif

            ScreenShakeManager.Instance.Shake();
            currentSpot.KillSpot();
            Destroy(gameObject);
        }
        else
        {
            if (isReadyToJump)
            {
                SFX.Play(1);
                isCaught = true;
                isReadyToJump = false;
                JumpToNewSpot();
                // ✅ Register juggle on successful bounce
                JuggleGameManager.Instance.RegisterJuggle();
            }
        }
    }

    public void JumpToNewSpot()
    {
        var availableSpots = SpotManager.Instance.GetAvailableSpots(currentSpot);
        if (availableSpots.Count == 0)
            return;

        SpotController targetSpot = availableSpots[Random.Range(0, availableSpots.Count)];

        currentSpot.currentObject = null;

        currentSpot = targetSpot;
        currentSpot.currentObject = this;

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

        isReadyToJump = true;
        isReadyToInteract = true;
    }

    public void LaunchToSpot(Vector3 targetPosition, float arcHeight, float jumpDuration)
    {
        StartCoroutine(JumpArc(targetPosition, arcHeight, jumpDuration));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTapped();
    }
}
