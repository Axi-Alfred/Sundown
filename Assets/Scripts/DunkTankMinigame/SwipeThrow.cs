using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeThrow : MonoBehaviour
{
    public GameObject ballPrefab; //Prefab for the ball
    public GameObject poofPrefab; //Prefab for the poof particle effect
    public GameObject ballPlaceholder; //Placeholder for the ball
    public Transform spawnPoint; //Startposition och spawn för bollen
    public float forceMultiplier = 0.05f; //Controls the force of the swipe throw

    //For storing the position where the touch starts and ends
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    private bool isSwipe; //Flag to detect if a swipe is in progress
    private bool hasBallInAir = false; // Prevents multiple balls in the air at the same time

    public Sprite[] ballSprites; // Array of possible sprites for the ball
    private Sprite nextBallSprite; // The sprite assigned to the next ball to be thrown
    private Coroutine fadeCoroutine; // Track and stop any ongoing fade animation

    // Sprite detector
    void Start()
    {
        // Randomly pick a sprite for the first ball
        if (ballSprites.Length > 0)
        {
            List<Sprite> validSprites = new List<Sprite>(ballSprites);
            validSprites.RemoveAll(sprite => sprite == null); // Avoid nulls

            if (validSprites.Count > 0)
                nextBallSprite = validSprites[Random.Range(0, validSprites.Count)];

            // Update placholder with the chosen sprite
            if (ballPlaceholder != null)
            {
                SpriteRenderer sr = ballPlaceholder.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = nextBallSprite;
            }
        }
    }

    // Input types
    void Update()
    {
        // Don't allow new swipe if a ball is already in the air
        if (hasBallInAir) return;

        // Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwipe = true;
            }

            if (touch.phase == TouchPhase.Ended && isSwipe)
            {
                endTouchPos = touch.position;
                Vector2 swipe = endTouchPos - startTouchPos;

                // Only count as a valid swipe if it moves upward
                if (swipe.y > 50)
                {
                    ThrowBall(swipe);
                }

                isSwipe = false;
            }
        }

#if UNITY_EDITOR

        // Mouse input (for editor testing)
        if (Input.GetMouseButtonDown(0))
            startTouchPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            Vector2 swipe = endTouchPos - startTouchPos;

            if (swipe.y > 50)
            {
                ThrowBall(swipe);
            }
        }
#endif
    }

    void ThrowBall(Vector2 swipe)
    {
        // Instantiate the ball at the spawn point
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        // Set the correct sprite on the new ball
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        if (sr != null && nextBallSprite != null)
            sr.sprite = nextBallSprite;

        // Apply force to the ball 
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(swipe.x, swipe.y) * forceMultiplier;
        rb.AddForce(force, ForceMode2D.Impulse);

        hasBallInAir = true;

        // Hide placeholder while the ball is in the air
        if (ballPlaceholder != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadePlaceholder(false));
        }

        // Attach the BallLifeTracker script when the ball is destroyed
        var tracker = ball.AddComponent<BallLifeTracker>();
        tracker.throwController = this;
        tracker.poofPrefab = poofPrefab;
    }

    // Called from BallLifeTracker when the ball is destroyed
    public void BallDestroyed()
    {
        hasBallInAir = false;

        // Pick new sprite for the next ball
        if (ballSprites.Length > 0)
        {
            List<Sprite> validSprites = new List<Sprite>(ballSprites);
            validSprites.RemoveAll(sprite => sprite == null);

            if (validSprites.Count > 0)
            {
                nextBallSprite = validSprites[Random.Range(0, validSprites.Count)];

                if (ballPlaceholder != null)
                {
                    SpriteRenderer sr = ballPlaceholder.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = nextBallSprite;
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f); // Start invisible

                        if (fadeCoroutine != null)
                            StopCoroutine(fadeCoroutine);

                        fadeCoroutine = StartCoroutine(FadePlaceholder(true)); // Fade back in
                    }
                }
            }
            else
            {
                Debug.LogWarning("No valid sprites to assign to placeholder.");
            }
        }
    }

    // Coroutine to fade the placeholder in or out smoothly
    IEnumerator FadePlaceholder(bool fadeIn, float duration = 0.2f)
    {
        if (ballPlaceholder == null) yield break;

        SpriteRenderer sr = ballPlaceholder.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        // Prevent fading in a null sprite
        if (fadeIn && sr.sprite == null)
        {
            Debug.LogWarning("Tried to fade in placeholder with null sprite.");
            yield break;
        }

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsed = 0f;

        Color color = sr.color;
        sr.color = new Color(color.r, color.g, color.b, startAlpha);

        ballPlaceholder.SetActive(true); // Ensure it's active 

        // Animate the fade
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, endAlpha);

        // Fully hide if fading out
        if (!fadeIn)
            ballPlaceholder.SetActive(false);
    }
}





