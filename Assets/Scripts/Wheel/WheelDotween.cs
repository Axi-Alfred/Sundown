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
    [SerializeField] private TMP_Text roundsText;

    [SerializeField] private GameObject playersInstructionsObject;

    [Header("New Round Texts")]
    [SerializeField] private GameObject newRoundTextObject;
    [SerializeField] TMP_Text currentRoundNumber;
    [SerializeField] TMP_Text previousRoundNumber;

    private void Awake()
    {
        spinWheel.enabled = false;
        roundsText.enabled = false;
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
        currentRoundNumber.text = GameManager1.currentRound.ToString();
        previousRoundNumber.text = (GameManager1.currentRound - 1).ToString();
        playerIcon.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
        playerName.text = PlayerData.currentPlayerTurn.PlayerName;
        Debug.Log("New Round: " + GameManager1.newRoundHasBegun);
    }

    private IEnumerator SceneInitialization()
    {
        yield return null;

        if (GameManager1.newRoundHasBegun)
        {
            yield return new WaitForSeconds(0.75f);
            yield return StartCoroutine(NewRoundDOTWeeen());
        }

        Sequence entranceSequence = DOTween.Sequence();


        RectTransform playerNameRT = playerName.gameObject.GetComponent<RectTransform>();
        RectTransform spinTextRT = spinText.gameObject.GetComponent<RectTransform>();


        playerNameRT.localScale = Vector3.one * 0.8f;
        spinTextRT.localScale = Vector3.one * 0.8f;

        entranceSequence.Join(playerNameRT.DOAnchorPosX(30f, 0.6f).SetEase(Ease.OutElastic, 0.9f, 0.25f)); 
        entranceSequence.Join(spinTextRT.DOAnchorPosX(-30f, 0.6f).SetEase(Ease.OutElastic, 0.9f, 0.25f)); 

        entranceSequence.Join(playerNameRT.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack));
        entranceSequence.Join(spinTextRT.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack));

        entranceSequence.Append(playerNameRT.DOAnchorPosX(0f, 0.2f).SetEase(Ease.InOutQuad));
        entranceSequence.Join(spinTextRT.DOAnchorPosX(0f, 0.2f).SetEase(Ease.InOutQuad));

        entranceSequence.Join(playerNameRT.DOScale(1f, 0.15f).SetEase(Ease.InOutQuad));
        entranceSequence.Join(spinTextRT.DOScale(1f, 0.15f).SetEase(Ease.InOutQuad));

        yield return entranceSequence.WaitForCompletion();
        yield return new WaitForSeconds(2f);

        Sequence exitSequence = DOTween.Sequence();

        exitSequence.Append(playerNameRT.DOAnchorPosX(40f, 0.2f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOAnchorPosX(-40f, 0.2f).SetEase(Ease.InQuad));

        exitSequence.Join(playerNameRT.DOScale(1.08f, 0.2f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOScale(1.08f, 0.2f).SetEase(Ease.InQuad));

        exitSequence.Append(playerNameRT.DOAnchorPosX(-1250f, 1).SetEase(Ease.OutElastic, 0.9f, 0.25f));
        exitSequence.Join(spinTextRT.DOAnchorPosX(1250f, 1).SetEase(Ease.OutElastic, 0.9f, 0.25f));

        exitSequence.Join(playerNameRT.DOScale(0.9f, 0.55f).SetEase(Ease.InQuad));
        exitSequence.Join(spinTextRT.DOScale(0.9f, 0.55f).SetEase(Ease.InQuad));

        yield return exitSequence.WaitForCompletion();
        yield return new WaitForSeconds(1f);

        playersInstructionsObject.SetActive(false);
        texts.gameObject.SetActive(true);
        roundsText.enabled = true;
        spinWheel.enabled = true;

    }

    private IEnumerator NewRoundDOTWeeen()
    {
        RectTransform textRT = newRoundTextObject.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0;

        if (GameManager1.currentRound == 1)
        {
            Sequence growInSequence = DOTween.Sequence();
            growInSequence.AppendCallback(() => newRoundTextObject.SetActive(true));
            growInSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
            growInSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
            growInSequence.AppendInterval(1.5f);

            yield return growInSequence.WaitForCompletion();
        }
        else
        {
            Color currentColor = currentRoundNumber.color;
            currentColor.a = 0;
            currentRoundNumber.color = currentColor;

            Vector3 startPos = currentRoundNumber.rectTransform.localPosition;
            currentRoundNumber.rectTransform.localPosition = new Vector3(startPos.x, startPos.y + 150f, startPos.z);

            previousRoundNumber.enabled = true;
            currentRoundNumber.enabled = true;

            Sequence growInSequence = DOTween.Sequence();
            growInSequence.AppendCallback(() => newRoundTextObject.SetActive(true));
            growInSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
            growInSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
            growInSequence.AppendInterval(0.5f);

            yield return growInSequence.WaitForCompletion();

            Sequence transitionSequence = DOTween.Sequence();

            transitionSequence.Join(previousRoundNumber.DOFade(0f, 0.6f));
            transitionSequence.Join(previousRoundNumber.rectTransform.DOLocalMoveY(-100f, 0.6f).SetEase(Ease.InQuad));

            yield return new WaitForSeconds(0.4f);

            transitionSequence.Join(currentRoundNumber.DOFade(1f, 0.6f));
            transitionSequence.Join(currentRoundNumber.rectTransform.DOLocalMoveY(startPos.y, 0.6f).SetEase(Ease.OutElastic));

            textRT.DOPunchPosition(new Vector3(0, -20, 0), 0.5f, 2);

            yield return transitionSequence.WaitForCompletion();

            yield return new WaitForSeconds(1.5f);
        }

        Sequence growOutSequence = DOTween.Sequence();
        growOutSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        growOutSequence.Append(textRT.DOScale(0f, 0.1f).SetEase(Ease.InOutQuad));
        growOutSequence.AppendInterval(0.75f);

        yield return growOutSequence.WaitForCompletion();

        GameManager1.newRoundHasBegun = false;
    }

}
