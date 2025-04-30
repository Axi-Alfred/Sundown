using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject poofPrefab;
    public GameObject ballPlaceholder;
    public Transform spawnPoint;
    public float forceMultiplier = 0.05f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwipe;
    private bool hasBallInAir = false;

    public Sprite[] ballSprites; // assign in Inspector
    private Sprite nextBallSprite;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (ballSprites.Length > 0)
        {
            List<Sprite> validSprites = new List<Sprite>(ballSprites);
            validSprites.RemoveAll(sprite => sprite == null);

            if (validSprites.Count > 0)
                nextBallSprite = validSprites[Random.Range(0, validSprites.Count)];

            if (ballPlaceholder != null)
            {
                SpriteRenderer sr = ballPlaceholder.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = nextBallSprite;
            }
        }
    }

    void Update()
    {
        if (hasBallInAir) return;

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

                if (swipe.y > 50)
                {
                    ThrowBall(swipe);
                }

                isSwipe = false;
            }
        }

#if UNITY_EDITOR
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
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        // Set the correct sprite
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        if (sr != null && nextBallSprite != null)
            sr.sprite = nextBallSprite;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(swipe.x, swipe.y) * forceMultiplier;
        rb.AddForce(force, ForceMode2D.Impulse);

        hasBallInAir = true;

        // Hide placeholder
        if (ballPlaceholder != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadePlaceholder(false));
        }

        var tracker = ball.AddComponent<BallLifeTracker>();
        tracker.throwController = this;
        tracker.poofPrefab = poofPrefab;
    }

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
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f); // start invisible

                        if (fadeCoroutine != null)
                            StopCoroutine(fadeCoroutine);

                        fadeCoroutine = StartCoroutine(FadePlaceholder(true));
                    }
                }
            }
            else
            {
                Debug.LogWarning("No valid sprites to assign to placeholder.");
            }
        }
    }
    IEnumerator FadePlaceholder(bool fadeIn, float duration = 0.2f)
    {
        if (ballPlaceholder == null) yield break;

        SpriteRenderer sr = ballPlaceholder.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

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

        ballPlaceholder.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, endAlpha);

        if (!fadeIn)
            ballPlaceholder.SetActive(false);
    }
}





