using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip machineHum;
    [SerializeField] private AudioClip cottonCandy;

    [Header("References")]
    public Transform stickTip;
    public Transform candyMachineCenter;
    [SerializeField] private GameObject cottonCandyObject;

    [Header("Growth Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float spawnThreshold = 90f;
    [SerializeField] private float scalePerSpinAlongStick = 0.03f;
    [SerializeField] private float scalePerSpinHorizontal = 0.01f;
    [SerializeField] private float winScale = 1.5f;
    [SerializeField] private Animator cottonCandyMachine;

    [Header("Parallax Settings")]
    [SerializeField] private Transform[] parallaxLayers;
    [SerializeField] private float parallaxStrength = 0.2f; // max movement multiplier
    [SerializeField] private Vector2 parallaxLimit = new Vector2(0.5f, 0.3f); // max movement in X/Y

    private AudioSource machineSource;
    private AudioSource playerInteractionSource;
    private Camera mainCam;


    private Vector2 lastDirection;
    private float rotationAccumulation = 0f;
    private bool hasWon = false;

    private Transform cottonCandyTransform;

    private bool hasTriggeredAnimation = false;


    void Start()
    {
        mainCam = Camera.main;

        // Sound
        machineSource = gameObject.AddComponent<AudioSource>();
        machineSource.clip = machineHum;
        machineSource.loop = true;
        machineSource.volume = 0.8f;
        machineSource.Play();

        playerInteractionSource = gameObject.AddComponent<AudioSource>();

        // Instantiate
        GameObject fluff = Instantiate(cottonCandyObject, stickTip.position, Quaternion.identity, stickTip);
        cottonCandyTransform = fluff.transform;


        cottonCandyTransform.localScale = Vector3.zero;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#else
        HandleMouseInput(); // fallback
#endif

        automaticSpinner();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            MoveStick(mouseWorldPos);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector3 touchWorldPos = mainCam.ScreenToWorldPoint(touch.position);
                touchWorldPos.z = 0f;
                MoveStick(touchWorldPos);
            }
        }
    }


    void MoveStick(Vector3 position)
    {
        transform.position = position;
        UpdateParallaxLayers();

        Vector2 toCenter = (Vector2)candyMachineCenter.position - (Vector2)stickTip.position;

        if (toCenter.magnitude <= detectionRadius)
        {
            Vector2 currentDirection = toCenter.normalized;

            if (!hasTriggeredAnimation)
            {
                cottonCandyMachine.SetTrigger("cottonCandy");
                hasTriggeredAnimation = true;
            }



            if (lastDirection != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(lastDirection, currentDirection);

                // ✅ Boost rotation sensitivity on mobile
                #if UNITY_ANDROID || UNITY_IOS
                angle *= 1.5f;
                #endif

                if (Mathf.Abs(angle) > 0.2f)
                {
                    rotationAccumulation += Mathf.Abs(angle);
                }
            }


            while (rotationAccumulation >= spawnThreshold)
            {
                CollectFluff();
                rotationAccumulation -= spawnThreshold;
            }

            lastDirection = currentDirection;
        }
        else
        {
            rotationAccumulation = 0f;
            lastDirection = Vector2.zero;

            cottonCandyMachine.SetBool("cottonCandy", false);

        }

        Debug.DrawLine(stickTip.position, candyMachineCenter.position, Color.magenta);
    }

    void CollectFluff()
    {
        if (cottonCandyTransform == null) return;

        // Play cotton candy sound, cotton candy sound will loop while collecting
        if (!playerInteractionSource.isPlaying)
        {
            playerInteractionSource.clip = cottonCandy;
            playerInteractionSource.loop = true;
            playerInteractionSource.Play();
        }


        // Get current scale
        Vector3 currentScale = cottonCandyTransform.localScale;
        Vector3 newScale = currentScale;

        // ✅ Grow along the stick's direction (mostly vertical)
        Vector3 growthDirection = stickTip.up; // Direction the stick tip is facing

        // Scale along the stick's direction
        newScale += growthDirection * scalePerSpinAlongStick;

        // ✅ Slight horizontal growth (fluff)
        newScale.x += scalePerSpinHorizontal;
        newScale.z += scalePerSpinHorizontal;

        cottonCandyTransform.localScale = newScale;
        cottonCandyTransform.localPosition = stickTip.up * (newScale.y / 2);

        // ✅ Win condition
        if (!hasWon && newScale.magnitude >= winScale)
        {
            hasWon = true;
            playerInteractionSource.Stop();
            PlayerData.currentPlayerTurn.AddScore(1);
            Debug.Log("You win!");
            Time.timeScale = 1f;
            GameManager1.EndTurn();
        }



    }
    void UpdateParallaxLayers()
    {
        Vector2 offset = (Vector2)(stickTip.position - candyMachineCenter.position);

        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            if (parallaxLayers[i] == null) continue;

            float depthFactor = (i + 1) / (float)parallaxLayers.Length;

            float xMove = Mathf.Clamp(offset.x * parallaxStrength * depthFactor, -parallaxLimit.x, parallaxLimit.x);
            float yMove = Mathf.Clamp(offset.y * parallaxStrength * depthFactor, -parallaxLimit.y, parallaxLimit.y);

            Vector3 targetOffset = new Vector3(xMove, yMove, 0f);
            parallaxLayers[i].localPosition = targetOffset;
        }
    }


    void automaticSpinner()
    {
        if (Input.GetKey(KeyCode.F))
        {
            float speed = 180f;
            transform.RotateAround(candyMachineCenter.position, Vector3.forward, speed * Time.deltaTime);
            transform.up = candyMachineCenter.position - stickTip.position;
        }
    }
}
