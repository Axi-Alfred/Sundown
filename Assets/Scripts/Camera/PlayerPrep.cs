using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPrep : MonoBehaviour
{
    public static PlayerPrep instance;

    [SerializeField] private GameObject entryPrefab; // Prefab för varje spelare
    [SerializeField] private GameObject iconContainer;
    [SerializeField] private TMP_Text playersText;
    [SerializeField] private GameObject startGameButton;

    private GameObject[] entriesArray;

    [Header("DOTweens")]
    [SerializeField] private float delayBetweenEntries = 0.1f;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startGameButton.SetActive(false);
        entriesArray = new GameObject[PlayerManager.Instance.numberOfPlayers];

        // Clear old entries
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        InitializePlayersList();
    }

    void Update()
    {
        if (iconContainer.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            iconContainer.SetActive(false);
        }
    }

    private void InitializePlayersList()
    {
        GameObject currentEntry;
        int i = 0;

        foreach (var player in PlayerManager.Instance.playersArray)
        {
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(transform);
            currentEntry.GetComponent<PlayerEntry>().Player = player;
            currentEntry.GetComponent<PlayerEntry>().LoadEntry();
            currentEntry.transform.localScale = Vector3.zero;

            entriesArray[i] = currentEntry;
            i++;
        }

        StartCoroutine(IconsFadeInDOTween());
    }

    public void ShowPicture(Player player)
    {
        iconContainer.SetActive(true);
        Image image = iconContainer.GetComponent<Image>();
        image.sprite = player.PlayerIcon;

        float imgWidth = image.sprite.rect.width;
        float imgHeight = image.sprite.rect.height;
        float aspectRatio = imgWidth / imgHeight;

        RectTransform rt = image.GetComponent<RectTransform>();
        float newHeight = rt.sizeDelta.y;
        float newWidth = newHeight * aspectRatio;
        rt.sizeDelta = new Vector2(newWidth, newHeight);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(IconsFadeOutDOTween());

        // Ensure ScenesController is found and nextSceneName is set
        var controller = FindObjectOfType<ScenesController>();
        if (controller != null)
        {
            controller.nextSceneName = "Wheel"; // ← ensures we always load the wheel scene
            controller.EndGameAndFadeOut();
        }
        else
        {
            Debug.LogError("ScenesController not found in scene!");
            SceneManager.LoadScene("Wheel"); // emergency fallback
        }
    }

    public IEnumerator IconsFadeOutDOTween()
    {
        float longestTweenDuration = 0.4f;

        for (int i = 0; i < entriesArray.Length; i++)
        {
            GameObject entryObject = entriesArray[i];
            RectTransform rt = entryObject.GetComponent<RectTransform>();
            Vector3 originalScale = rt.localScale;

            DOTween.Sequence()
                .AppendInterval(i * delayBetweenEntries)
                .Append(rt.DOScale(originalScale * 1.2f, 0.15f).SetEase(Ease.OutQuad))
                .Append(rt.DOScale(originalScale * 0.1f, 0.25f).SetEase(Ease.InQuad))
                .OnComplete(() => entryObject.transform.localScale = Vector3.zero);
        }

        float totalDuration = (entriesArray.Length - 1) * delayBetweenEntries + longestTweenDuration;
        yield return new WaitForSeconds(totalDuration - 0.25f);
    }

    public IEnumerator IconsFadeInDOTween()
    {
        playersText.enabled = false;
        yield return new WaitForSeconds(0.75f);

        RectTransform textRT = playersText.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        DOTween.Sequence()
            .AppendCallback(() => playersText.enabled = true)
            .Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack))
            .Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad))
            .AppendInterval(1.5f)
            .Join(textRT.DOAnchorPosY(300, 0.6f).SetEase(Ease.OutQuad));

        yield return new WaitForSeconds(0.5f);

        float longestTweenDuration = 0.4f;

        for (int i = 0; i < entriesArray.Length; i++)
        {
            GameObject entryObject = entriesArray[i];
            RectTransform rt = entryObject.GetComponent<RectTransform>();

            rt.localScale = Vector3.one * 0.1f;

            DOTween.Sequence()
                .AppendInterval(i * delayBetweenEntries)
                .Append(rt.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutQuad))
                .Append(rt.DOScale(Vector3.one, 0.15f).SetEase(Ease.InQuad));
        }

        yield return new WaitForSeconds(2.5f);
        startGameButton.SetActive(true);
    }
}
