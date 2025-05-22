using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Image playerSprite;
    private bool wheelHasSpinned;

    [SerializeField] private float gameStartTimer = 2.5f;

    [SerializeField] private TMP_Text nextGameText;

    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        playerText.text = "Now spinning: " + PlayerData.currentPlayerTurn.PlayerName;
        roundText.text = GameManager1.currentRound == PlayerData.numberOfRounds
            ? "Round " + GameManager1.currentRound + ", Final Round"
            : "Round " + GameManager1.currentRound;

        playerSprite.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!nextGameText.gameObject.activeSelf) nextGameText.gameObject.SetActive(true);
        StartCoroutine(GameTextPopupDOTween());
        ScreenShake.instance.TriggerShake();

        string tag = other.tag;
        StartCoroutine(Timer(tag));
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }

    IEnumerator Timer(string tag)
    {
        yield return new WaitForSeconds(gameStartTimer);

        string sceneName = SpinWheel.GetSceneForTag(tag);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[Pointer] Scene name for tag '{tag}' not found!");
            yield break;
        }

        var transition = FindObjectOfType<SceneTransition>();
        if (transition != null)
            transition.StartFadeOut(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    private IEnumerator GameTextPopupDOTween()
    {
        RectTransform textRT = nextGameText.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => nextGameText.gameObject.SetActive(true));
        textSequence.Append(textRT.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);

        yield return null;
    }
}
