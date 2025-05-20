using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpinWheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private float wheelMotionlessThreshold = 0.5f;
    [SerializeField] private float torqueMultiplier = 50f;
    [SerializeField] private float minDragDistance;
    [SerializeField] private float minSpinForce;
    [SerializeField] private Pointer pointer;
    [SerializeField] private GameObject pointerObject;

    [Header("Pie Setup")]
    [SerializeField] private GameObject[] wheelSlices; // Assign each pie GameObject manually

    [Header("Minigame Settings")]
    [SerializeField] private float sceneLoadDelay = 1.5f;

    private Rigidbody2D rb2D;
    private Vector2 lastTouchPos;
    private Vector2 wheelCenter;
    private bool isDragging;
    private bool isSpinning;
    private bool hasSpinned;
    private bool hasReachedMotionThreshold;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        wheelCenter = Camera.main.WorldToScreenPoint(transform.position);
        float wheelRotation = Random.Range(1, 90);
        transform.localRotation = Quaternion.Euler(0, 0, wheelRotation);
    }

    void Update()
    {
        float wheelVelocity = Mathf.Abs(rb2D.angularVelocity);

        if (isSpinning)
        {
            if (!hasReachedMotionThreshold && wheelVelocity > wheelMotionlessThreshold)
            {
                hasReachedMotionThreshold = true;
            }

            if (hasReachedMotionThreshold && wheelVelocity < wheelMotionlessThreshold && Mathf.Abs(pointerObject.transform.eulerAngles.z) < 5)
            {
                rb2D.angularVelocity = 0;
                isSpinning = false;
                hasSpinned = true;
                hasReachedMotionThreshold = false;
                Handheld.Vibrate();

                StartCoroutine(LaunchMinigameByPointer());
            }
        }

        if (Input.touchCount > 0 && !isSpinning && !hasSpinned)
        {
            SpinTheWheel(Input.GetTouch(0));
        }

        pointer.WheelHasSpinned(hasSpinned);
    }

    private void SpinTheWheel(Touch touch)
    {
        if (hasSpinned || isSpinning) return;

        if (touch.phase == TouchPhase.Began)
        {
            isDragging = true;
            lastTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 currentTouchPos = touch.position;
            float dragDistance = Vector2.Distance(currentTouchPos, lastTouchPos);
            if (dragDistance < minDragDistance) return;

            Vector2 from = lastTouchPos - wheelCenter;
            Vector2 to = currentTouchPos - wheelCenter;
            float angle = Vector2.SignedAngle(from, to);
            float touchVelocity = touch.deltaPosition.magnitude / touch.deltaTime;
            float torque = Mathf.Max(minSpinForce, Mathf.Abs(angle * touchVelocity * torqueMultiplier));

            rb2D.AddTorque(-angle * -torque);

            lastTouchPos = currentTouchPos;
            isSpinning = true;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;
        }
    }

    public void SpinWithButton()
    {
        int randomSpinForce = Random.Range(500, 1000);
        rb2D.AddTorque(-randomSpinForce);
        isSpinning = true;
    }

    public void ResetWheel()
    {
        rb2D.angularVelocity = 0;
        rb2D.rotation = 0f;
        hasSpinned = false;
        isSpinning = false;
        hasReachedMotionThreshold = false;
    }

    private IEnumerator LaunchMinigameByPointer()
    {
        yield return new WaitForSeconds(sceneLoadDelay);

        GameObject closest = null;
        float closestAngle = 999f;

        foreach (var slice in wheelSlices)
        {
            Vector3 direction = slice.transform.position - pointerObject.transform.position;
            float angle = Vector3.Angle(pointerObject.transform.up, direction);

            if (angle < closestAngle)
            {
                closestAngle = angle;
                closest = slice;
            }
        }

        if (closest == null)
        {
            Debug.LogError("[SpinWheel] No slice found!");
            yield break;
        }

        string tag = closest.tag; // e.g., "Game7"
        if (!tag.StartsWith("Game"))
        {
            Debug.LogError("[SpinWheel] Invalid tag: " + tag);
            yield break;
        }

        string numberPart = tag.Substring(4); // remove "Game"
        if (!int.TryParse(numberPart, out int sceneIndex))
        {
            Debug.LogError("[SpinWheel] Cannot parse scene index from tag: " + tag);
            yield break;
        }

        Debug.Log($"[SpinWheel] Launching scene index {sceneIndex} via tag {tag}");

        SceneTransition transition = FindObjectOfType<SceneTransition>();
        if (transition != null)
        {
            transition.StartFadeOut(sceneIndex.ToString());
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
