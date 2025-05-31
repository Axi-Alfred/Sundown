using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    [Header("Ljud")]
    [SerializeField] private AudioClip machineHum;
    [SerializeField] private AudioClip cottonCandy;

    [Header("Referenser")]
    public Transform stickTip;
    public Transform candyMachineCenter;
    [SerializeField] private GameObject cottonCandyObject;

    [Header("Inställningar för sockervadden")]
    [SerializeField] private float detectionRadius = 2f; // HUr nära behöver vi vara för att trigga sockervaddsskapandet?
    [SerializeField] private float spawnThreshold = 90f;
    [SerializeField] private float scalePerSpinAlongStick = 0.03f;
    [SerializeField] private float scalePerSpinHorizontal = 0.01f;
    [SerializeField] private float winScale = 1.5f;
    [SerializeField] private Animator cottonCandyMachine;

    [Header("Parallax inställningar")]
    [SerializeField] private Transform[] parallaxLayers;
    [SerializeField] private float parallaxStrength = 0.2f;
    [SerializeField] private Vector2 parallaxLimit = new Vector2(0.5f, 0.3f);

    private AudioSource machineSource;
    private AudioSource playerInteractionSource;
    private Camera mainCam;
    private Transform cottonCandyTransform;

    private Vector2 lastDirection;

    private float rotationAccumulation = 0f;
    private bool hasWon = false;
    private bool hasTriggeredAnimation = false;


    void Start()
    {
        mainCam = Camera.main;

        machineSource = gameObject.AddComponent<AudioSource>();
        machineSource.clip = machineHum;
        machineSource.loop = true;
        machineSource.volume = 0.8f;
        machineSource.Play();

        playerInteractionSource = gameObject.AddComponent<AudioSource>();

        GameObject fluff = Instantiate(cottonCandyObject, stickTip.position, Quaternion.identity, stickTip);
        cottonCandyTransform = fluff.transform;


        cottonCandyTransform.localScale = Vector3.zero;
    }

    void Update() // Hantera input baserat på platform
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#else
        HandleMouseInput();
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
        // Flyttar stickan och uppdaterar parrallax

        transform.position = position;
        UpdateParallaxLayers(); // Ska följa sockervaddsPinnen

        Vector2 toCenter = (Vector2)candyMachineCenter.position - (Vector2)stickTip.position;

        if (toCenter.magnitude <= detectionRadius)
        {
            Vector2 currentDirection = toCenter.normalized;

            if (!hasTriggeredAnimation) // maskinen ska animeras endast en gång
            {
                cottonCandyMachine.SetTrigger("cottonCandy");
                hasTriggeredAnimation = true;
            }



            if (lastDirection != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(lastDirection, currentDirection);

                #if UNITY_ANDROID || UNITY_IOS
                angle *= 1.5f; // det ska samlas sockervadd lite fortare på touchskärm. Angle = totala rotationen

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

        if (!playerInteractionSource.isPlaying)
        {
            playerInteractionSource.clip = cottonCandy;
            playerInteractionSource.loop = true;
            playerInteractionSource.Play();
        }


        Vector3 currentScale = cottonCandyTransform.localScale;
        Vector3 newScale = currentScale;

        Vector3 growthDirection = stickTip.up;

        newScale += growthDirection * scalePerSpinAlongStick;

        newScale.x += scalePerSpinHorizontal;
        newScale.z += scalePerSpinHorizontal;

        cottonCandyTransform.localScale = newScale;
        cottonCandyTransform.localPosition = stickTip.up * (newScale.y / 2);

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


    void automaticSpinner() // Endast för debugging
    {
        if (Input.GetKey(KeyCode.F))
        {
            float speed = 180f;
            transform.RotateAround(candyMachineCenter.position, Vector3.forward, speed * Time.deltaTime);
            transform.up = candyMachineCenter.position - stickTip.position;
        }
    }
}