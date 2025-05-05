using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    [SerializeField] private AudioClip machineHum, crowdSounds;
    public Transform stickTip; // Pink circle
    public Transform candyMachineCenter; // Center of white circle
    public float detectionRadius = 2f; // how close the stick tip has to be to start rotating.
    public GameObject cottonCandyPrefab;

    private Vector2 lastDirection; // last direction from stick tip Å® center.
    private float rotationAccumulation = 0f; // keeps track of how much you've rotated.
    private float spawnThreshold = 360f; // spawn candy after 360 degrees of rotation.
    private float victoryRotation;
    private float autoSpinAngle;
    private AudioSource machineSource;
    private AudioSource crowdSource;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        // Create and configure both sources
        machineSource = gameObject.AddComponent<AudioSource>();
        machineSource.clip = machineHum;
        machineSource.loop = true;
        machineSource.volume = 0.8f; // louder
        machineSource.Play();

        crowdSource = gameObject.AddComponent<AudioSource>();
        crowdSource.clip = crowdSounds;
        crowdSource.loop = true;
        crowdSource.volume = 0.6f; // quieter
        crowdSource.Play();
    }

    void Update() // Chooses input method depending on platform: mouse or touch.
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
        if (Input.GetMouseButton(0))  // If the left mouse button is held... Converts the 2D mouse position to a 3D world position, locks Z (because it's 2D), and moves the stick.
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            MoveStick(mouseWorldPos);
        }
    }

    void HandleTouchInput() // Gets touch position, converts to world coordinates, and moves the stick.
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
                rotationAccumulation += Mathf.Abs(angle);
            }

            while (rotationAccumulation >= spawnThreshold)
            {
                SpawnCottonCandy();
                rotationAccumulation -= spawnThreshold;
            }

            if (rotationAccumulation > 600f)
            {
                Debug.Log("You win!");
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

    void SpawnCottonCandy()
    {
        // Random slight offset to make it build up organically
        Vector2 offset = Random.insideUnitCircle * 0.2f;

        // Position relative to the stick tip
        Vector3 spawnPosition = stickTip.position + (Vector3)offset;

        // Instantiate and parent to the stick tip
        GameObject cottonPiece = Instantiate(cottonCandyPrefab, spawnPosition, Quaternion.identity, stickTip);

        // Optional: Random rotation or scale for variation
        cottonPiece.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        cottonPiece.transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
    }
    void automaticSpinner()
    {
        if (Input.GetKey(KeyCode.F))
        {
            float speed = 180f; // degrees per second
            transform.RotateAround(candyMachineCenter.position, Vector3.forward, speed * Time.deltaTime);

            // Optional: make the stick face the center
            transform.up = candyMachineCenter.position - stickTip.position;
        }
    }

}

