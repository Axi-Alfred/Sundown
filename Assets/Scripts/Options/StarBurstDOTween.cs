using UnityEngine;
using DG.Tweening;

public class StarBurstDOTween : MonoBehaviour
{
    public RectTransform canvasRect;
    public GameObject starPrefab;
    public int starCount = 20;
    public AudioSource chime;

    void Awake()
    {
        if (canvasRect == null)
        {
            Canvas foundCanvas = FindObjectOfType<Canvas>();
            if (foundCanvas != null)
            {
                canvasRect = foundCanvas.GetComponent<RectTransform>();
                Debug.Log("[StarBurstDOTween] Auto-assigned canvas: " + foundCanvas.name);
            }
            else
            {
                Debug.LogWarning("[StarBurstDOTween] No Canvas found in scene! Star burst will not play.");
            }
        }
    }

    [ContextMenu("▶ TEST Star Burst (Inspector)")]
    public void TriggerBurst()
    {
        if (canvasRect == null)
        {
            Debug.LogWarning("[StarBurstDOTween] Cannot trigger burst: no Canvas assigned.");
            return;
        }

        float w = canvasRect.rect.width / 2;
        float h = canvasRect.rect.height / 2;

        for (int i = 0; i < starCount; i++)
        {
            GameObject starObj = Instantiate(starPrefab, canvasRect);
            RectTransform starRect = starObj.GetComponent<RectTransform>();
            CanvasGroup cg = starObj.GetComponent<CanvasGroup>();

            // Spawn offscreen
            Vector2 startPos = GetRandomOutsidePosition(w, h);
            starRect.anchoredPosition = startPos;

            // Target halfway between center and edge, with randomness
            Vector2 randomOffset = new Vector2(Random.Range(-w * 0.5f, w * 0.5f), Random.Range(-h * 0.5f, h * 0.5f));
            Vector2 targetPos = randomOffset;

            // Animate movement
            starRect.DOAnchorPos(targetPos, 1f).SetEase(Ease.InOutQuad);

            // Animate scale: pop in, shrink slightly
            starRect.localScale = Vector3.one * Random.Range(1.2f, 1.8f);
            starRect.DOScale(Vector3.one * Random.Range(0.8f, 1.2f), 1f).SetEase(Ease.InOutQuad);

            // Animate spin
            starRect.DORotate(new Vector3(0, 0, Random.Range(-360, 360)), 1f, RotateMode.FastBeyond360);

            // Animate fade out
            cg.alpha = 1;
            cg.DOFade(0, 0.5f).SetDelay(0.5f).OnComplete(() => Destroy(starObj));
        }

        if (chime != null)
            chime.Play();
    }

    private Vector2 GetRandomOutsidePosition(float w, float h)
    {
        float offset = 100f;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: return new Vector2(Random.Range(-w, w), h + offset);      // Above top
            case 1: return new Vector2(Random.Range(-w, w), -h - offset);     // Below bottom
            case 2: return new Vector2(-w - offset, Random.Range(-h, h));     // Left
            case 3: return new Vector2(w + offset, Random.Range(-h, h));      // Right
            default: return Vector2.zero;
        }
    }
}
