using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class WheelDotween : MonoBehaviour
{
    [SerializeField] private GameObject texts;
    [SerializeField] private SpinWheel spinWheel;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image playerIcon;

    [SerializeField] private TMP_Text spinText;

    [SerializeField] private GameObject playersInstructionsObject;

    private void Awake()
    {
        spinWheel.enabled = false;
        texts.gameObject.SetActive(false);  
    }
    // Start is called before the first frame update
    void Start()
    {
        playersInstructionsObject.SetActive(true);
        StartCoroutine(SceneInitialization());
    }

    // Update is called once per frame
    void Update()
    {
        playerIcon.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
        playerName.text = PlayerData.currentPlayerTurn.PlayerName;
    }

    private IEnumerator SceneInitialization()
    {
        yield return new WaitForSeconds(2f);

        Sequence entranceSequence = DOTween.Sequence();


        RectTransform playerNameRT = playerName.gameObject.GetComponent<RectTransform>();
        RectTransform spinTextRT = spinText.gameObject.GetComponent<RectTransform>();

        playerNameRT.localScale = Vector3.one * 0.8f;
        spinTextRT.localScale = Vector3.one * 0.8f;

        entranceSequence.Join(playerNameRT.DOAnchorPosX(30f, 0.6f).SetEase(Ease.OutQuint)); 
        entranceSequence.Join(spinTextRT.DOAnchorPosX(-30f, 0.6f).SetEase(Ease.OutQuint)); 

        // Scale up slightly bigger than final size
        entranceSequence.Join(playerNameRT.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack));
        entranceSequence.Join(spinTextRT.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack));

        // Move back to exact position
        entranceSequence.Append(playerNameRT.DOAnchorPosX(0f, 0.2f).SetEase(Ease.InOutQuad));
        entranceSequence.Join(spinTextRT.DOAnchorPosX(0f, 0.2f).SetEase(Ease.InOutQuad));

        // Scale to normal
        entranceSequence.Join(playerNameRT.DOScale(1f, 0.15f).SetEase(Ease.InOutQuad));
        entranceSequence.Join(spinTextRT.DOScale(1f, 0.15f).SetEase(Ease.InOutQuad));

        yield return entranceSequence.WaitForCompletion();
        yield return new WaitForSeconds(2f);

        Sequence exitSequence = DOTween.Sequence();

        exitSequence.Append(playerNameRT.DOAnchorPosX(40f, 0.2f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOAnchorPosX(-40f, 0.2f).SetEase(Ease.InQuad));

        exitSequence.Join(playerNameRT.DOScale(1.08f, 0.2f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOScale(1.08f, 0.2f).SetEase(Ease.InQuad));

        exitSequence.Append(playerNameRT.DOAnchorPosX(-1250f, 0.55f).SetEase(Ease.InCubic));
        exitSequence.Join(spinTextRT.DOAnchorPosX(1250f, 0.55f).SetEase(Ease.InCubic));

        exitSequence.Join(playerNameRT.DOScale(0.9f, 0.55f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOScale(0.9f, 0.55f).SetEase(Ease.InQuad));

        yield return exitSequence.WaitForCompletion();
        yield return new WaitForSeconds(1f);

        playersInstructionsObject.SetActive(false);
        texts.gameObject.SetActive(true);
        spinWheel.enabled = true;

    }
}
