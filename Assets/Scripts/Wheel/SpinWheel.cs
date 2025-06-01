using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    public new string tag {  get; private set; }

    [Header("Wheel Settings")]
    [SerializeField] private float wheelMotionlessThreshold = 0.5f;
    [SerializeField] private float torqueMultiplier = 50f;
    [SerializeField] private float minDragDistance;
    [SerializeField] private float minSpinForce;
    [SerializeField] private Pointer pointer;
    [SerializeField] private GameObject pointerObject;

    [Header("Pie Setup")]
    [SerializeField] private GameObject[] wheelSlices;

    [Header("Minigame Settings")]
    [SerializeField] private float sceneLoadDelay = 1.5f;

    [Header("Editor Testing")]
    [SerializeField] private bool allowKeyboardSpin = true;
    [SerializeField] private KeyCode spinKey = KeyCode.Space;
    [SerializeField] private GameObject keyboardHintText;


    private Rigidbody2D rb2D;
    private Vector2 lastTouchPos;
    private Vector2 wheelCenter;
    private bool isDragging;
    private bool isSpinning;
    private bool hasSpinned;
    private bool hasReachedMotionThreshold;

    public static readonly Dictionary<string, string> tagToScene = new()
{
    { "Game1", "X 3IsItRight" },
    { "Game2", "X 4OddOneOut" },
    { "Game3", "X 5PopTheBalloon" },
    { "Game4", "X 7JuggleMania" },
    { "Game5", "X 9SmackedPig" },
    { "Game6", "X 10CottonCandy" },
    { "Game7", "X 11SaveTheClowns" },
    { "Game8", "X 12ClownElope" },
    { "Game9", "X 13DunkTank" },
    { "Game10", "X 14DontGetHit" },
    { "Game11", "X 15CatchHop_Main" },
};

    public static readonly Dictionary<string, string> tagToDisplayName = new()
{
    { "Game1", "Is It Right?" },
    { "Game2", "Odd One Out" },
    { "Game3", "Pop The Balloon" },
    { "Game4", "Juggle Mania" },
    { "Game5", "Pig?" },
    { "Game6", "Cotton Candy" },
    { "Game7", "Save the Clowns" },
    { "Game8", "Clown Elope" },
    { "Game9", "Dunk Tank" },
    { "Game10", "Don't Get Hit" },
    { "Game11", "Catch Hop" },
    { "Game12", "Random Game!" },
    { "Game13", "Random Game!" }
};



    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        wheelCenter = Camera.main.WorldToScreenPoint(transform.position);
        float wheelRotation = Random.Range(1, 360);
        transform.localRotation = Quaternion.Euler(0, 0, wheelRotation);
        Debug.Log(wheelRotation);
    }

    void Update()
    {
        float wheelVelocity = Mathf.Abs(rb2D.angularVelocity);

        if (isSpinning)
        {
            if (!hasReachedMotionThreshold && wheelVelocity > wheelMotionlessThreshold)
                hasReachedMotionThreshold = true;

            if (hasReachedMotionThreshold && wheelVelocity < wheelMotionlessThreshold && Mathf.Abs(pointerObject.transform.eulerAngles.z) < 5)
            {
                rb2D.angularVelocity = 0;
                isSpinning = false;
                hasSpinned = true;
                hasReachedMotionThreshold = false;

#if UNITY_ANDROID || UNITY_IOS
Handheld.Vibrate();
#endif

                StartCoroutine(LaunchMinigameByPointer());
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (allowKeyboardSpin && Input.GetKeyDown(spinKey) && !isSpinning && !hasSpinned)
        {
            SpinWithButton();
            if (keyboardHintText != null)
                keyboardHintText.SetActive(false); // Dölj när man snurrar
        }
#endif

        if (Input.touchCount > 0 && !isSpinning && !hasSpinned)
        {
            SpinTheWheel(Input.GetTouch(0));
            if (keyboardHintText != null)
                keyboardHintText.SetActive(false); // Dölj även om man snurrar via touch
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
            if (slice == null)
            {
                Debug.LogWarning("[SpinWheel] Null slice found in wheelSlices.");
                continue;
            }

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

        tag = closest.tag;
        Debug.Log($"[SpinWheel] Slice tag: {tag}");

        if (tagToDisplayName.TryGetValue(tag, out string displayName))
        {
            var ui = FindObjectOfType<WheelDotween>();
            if (ui != null)
                ui.ShowChosenGameName(displayName);
        }


        string sceneName;

        if (tag == "RandomGame")
        {
            List<string> scenes = new List<string>(tagToScene.Values);
            sceneName = scenes[Random.Range(0, scenes.Count)];
            Debug.Log($"[SpinWheel] RANDOM pick → {sceneName}");
        }
        else if (!tagToScene.TryGetValue(tag, out sceneName))
        {
            Debug.LogError($"[SpinWheel] Unknown tag: {tag}");
            yield break;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SpinWheel] Scene name is EMPTY. Aborting spin.");
            yield break;
        }

        Debug.Log($"[SpinWheel] Launching scene: {sceneName}");

        ScenesController controller = FindObjectOfType<ScenesController>();
        if (controller != null)
        {
            controller.nextSceneName = sceneName;
            Debug.Log($"[SpinWheel] Setting nextSceneName to: {controller.nextSceneName}");
            controller.TriggerEndNow();
        }
        else
        {
            Debug.LogError("[SpinWheel] ScenesController not found!");
        }
    }
    public static string GetDisplayNameForTag(string tag)
    {
        return tagToDisplayName.ContainsKey(tag) ? tagToDisplayName[tag] : "Unknown Game";
    }

    public static string GetSceneForTag(string tag)
    {
        if (tag == "Game12" || tag == "Game13" || tag == "RandomGame")
        {
            // Pick a real random scene from the valid list
            List<string> validScenes = new List<string>(tagToScene.Values);
            return validScenes[Random.Range(0, validScenes.Count)];
        }

        return tagToScene.ContainsKey(tag) ? tagToScene[tag] : string.Empty;
    }


}
