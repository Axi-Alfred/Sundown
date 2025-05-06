using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip machineHum, crowdSounds;
    // kids laughing

    [Header("References")]
    public Transform stickTip;
    public Transform candyMachineCenter;
    [SerializeField] private GameObject cottonCandyObject;

    [Header("Growth Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float spawnThreshold = 90f; // faster feedback
    [SerializeField] private float scalePerSpin = 0.01f;
    [SerializeField] private float winScale = 1.2f;

    private AudioSource machineSource;
    private AudioSource crowdSource;
    private Camera mainCam;

    private Vector2 lastDirection;
    private float rotationAccumulation = 0f;
    private Transform cottonCandyTransform;

    void Start()
    {
        mainCam = Camera.main;

        // Sound
        machineSource = gameObject.AddComponent<AudioSource>();
        machineSource.clip = machineHum;
        machineSource.loop = true;
        machineSource.volume = 0.8f;
        machineSource.Play();

        crowdSource = gameObject.AddComponent<AudioSource>();
        crowdSource.clip = crowdSounds;
        crowdSource.loop = true;
        crowdSource.volume =1f;
        crowdSource.Play();

        // Instantiate single cotton fluff object
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
            Vector3 touchWorldPos = mainCam.ScreenToWorldPoint(touch.position);
            touchWorldPos.z = 0f;
            MoveStick(touchWorldPos);
        }
    }

    void MoveStick(Vector3 position)
    {
        transform.position = position;

        Vector2 toCenter = (Vector2)candyMachineCenter.position - (Vector2)stickTip.position;

        if (toCenter.magnitude <= detectionRadius)
        {
            Vector2 currentDirection = toCenter.normalized;

            if (lastDirection != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(lastDirection, currentDirection);

                if (Mathf.Abs(angle) > 0.5f)
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
        }

        Debug.DrawLine(stickTip.position, candyMachineCenter.position, Color.magenta);
    }

    void CollectFluff()
    {
        if (cottonCandyTransform == null) return;

        Vector3 currentScale = cottonCandyTransform.localScale;
        Vector3 newScale = currentScale + Vector3.one * scalePerSpin;

        cottonCandyTransform.localScale = newScale;

        if (newScale.x >= winScale)
        {
            PlayerData.currentPlayerTurn.AddScore(1);

            Debug.Log("You win!");
            GameManager1.EndTurn();
            
            // Trigger visual or game flow change here
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
