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

    [SerializeField] private GameObject entryPrefab; //Prefaben f�r varje individuellt entry av spelare
    [SerializeField] private GameObject iconContainer;
    [SerializeField] private TMP_Text playersText;
    [SerializeField] private GameObject startGameButton;

    private GameObject[] entriesArray;

    [Header("DOTweens")]
    [SerializeField] private float delayBetweenEntries = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.SetActive(false);

        entriesArray = new GameObject[PlayerData.numberOfPlayers];

        //Loopen är till att ta bort alla tidigare entries i leaderboarden innan man skapar de nya
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        InitializePlayersList();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (iconContainer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                iconContainer.SetActive(false);
            }
        }
    }

    private void InitializePlayersList()
    {
        GameObject currentEntry;
        int i = 0;

        foreach (var player in PlayerData.playersArray)
        {
            currentEntry = null;
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(gameObject.transform);
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
        RectTransform rt = image.GetComponent<RectTransform>();
        float aspectRatio = imgWidth / imgHeight;
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
        SceneTransition transition = GameObject.FindObjectOfType<SceneTransition>();
        if (transition != null)
        {
            transition.StartFadeOut("Wheel");
        }
        else
        {
            Debug.LogWarning("[MenuController] SceneTransition not found, loading scene directly.");
            SceneManager.LoadScene("Wheel");
        }
    }

    public IEnumerator IconsFadeOutDOTween()
    {
        float longestTweenDuration = 0.5f;

        for (int i = 0; i < entriesArray.Length; i++)
        {
            GameObject entryObject = entriesArray[i];
            RectTransform rt = entryObject.GetComponent<RectTransform>();
            Vector3 originalScale = rt.localScale;

            DOTween.Sequence().AppendInterval(i * delayBetweenEntries).Append(rt.DOScale(originalScale * 1.2f, 0.15f).SetEase(Ease.OutQuad)).Append(rt.DOScale(originalScale * 0.1f, 0.25f).SetEase(Ease.InQuad)).OnComplete(() => entryObject.transform.localScale = Vector3.zero);
        }

        float totalDuration = (entriesArray.Length - 1) * delayBetweenEntries + longestTweenDuration;
        yield return new WaitForSeconds(totalDuration);
    }

    public IEnumerator IconsFadeInDOTween()
    {
        //Text tweens
        playersText.enabled = false;
        yield return new WaitForSeconds(0.75f);

        RectTransform textRT = playersText.gameObject.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => playersText.enabled = true);
        textSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);
        textSequence.Join(textRT.DOAnchorPosY(300, 0.6f).SetEase(Ease.OutQuad));

        yield return new WaitForSeconds(0.5f);

        float longestTweenDuration = 0.4f;

        for (int i = 0; i < entriesArray.Length; i++)
        {
            GameObject entryObject = entriesArray[i];
            RectTransform rt = entryObject.GetComponent<RectTransform>();
            Vector3 originalScale = rt.localScale;

            rt.localScale = Vector3.one * 0.1f;

            Sequence s = DOTween.Sequence();
            s.AppendInterval(i * delayBetweenEntries);
            SFXLibrary.Instance.Play(3);
            s.Append(rt.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutQuad));
            s.Append(rt.DOScale(Vector3.one, 0.15f).SetEase(Ease.InQuad));
        }

        float totalDuration = (entriesArray.Length - 1) * delayBetweenEntries + longestTweenDuration;

        yield return new WaitForSeconds(2.5f);

        startGameButton.SetActive(true);
    }

}

