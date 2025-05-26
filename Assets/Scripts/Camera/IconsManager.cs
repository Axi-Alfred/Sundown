using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using DG.Tweening;
using static System.Net.Mime.MediaTypeNames;

public class IconsManager : MonoBehaviour
{
    [SerializeField] private AnimationClip countdown;

    [SerializeField] public Filter[] filtersArray;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text instructions;
    [SerializeField] private GameObject nextPlayerButton;
    [SerializeField] private GameObject continueButton;

    private FrontCamera frontCamera;
    private Animator animator;


    [NonSerialized] public List<Filter> tempFiltersList;
    [NonSerialized] public Filter currentFilter;
    [NonSerialized] public Player currentPlayer;

    private bool hasTakenPic;
    private bool hasContinued;

    private int countdownNumber = 5;
    [SerializeField] private TMP_Text countdownNumberText;
    [SerializeField] private GameObject countdownNumberObject;

    [SerializeField] private GameObject playersListPanel;
    [SerializeField] private GameObject InitialPopup;
    [SerializeField] private GameObject initialPopupText;

    private string[] playerNumbersStrings = new string[4] { "one", "two", "three", "four" };

    // Start is called before the first frame update
    void Start()
    {
        frontCamera = GetComponent<FrontCamera>();
        animator = GetComponent<Animator>();
        initialPopupText.SetActive(false);

        playersListPanel.SetActive(false);


        //Create a random order of filters handed out so no two players can get the same filter per round
        tempFiltersList = filtersArray.ToList();
        tempFiltersList = tempFiltersList.OrderBy(p => Random.value).ToList();

        hasTakenPic = false;

        StartCoroutine(TakePlayerPictureLoop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TakePlayerPictureLoop()
    {
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(InitialTextDOTween());

        yield return new WaitForSeconds(4f);

        InitialPopup.SetActive(false);

        for (int i = 0; i < PlayerData.playersArray.Length; i++)
        {
            hasTakenPic = false;
            instructions.gameObject.SetActive(true);
            countdownNumberObject.SetActive(true);
            playerName.gameObject.SetActive(false);
            nextPlayerButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            currentFilter = tempFiltersList[i];
            instructions.text = "Player number " + playerNumbersStrings[i] + ", show us what you got!";
            currentPlayer = PlayerData.playersArray[i];
            animator.SetBool("PlayCountdown", true);

            yield return new WaitForSeconds(countdown.length);

            StartCoroutine(frontCamera.TakePicture());
            SFXLibrary.Instance.Play(1);

            countdownNumberObject.SetActive(false);
            instructions.gameObject.SetActive(false);
            playerName.gameObject.SetActive(true);
            playerName.text = "Your name is " + currentFilter.filterName;
            animator.SetBool("PlayCountdown", false);

            if (i == (PlayerData.playersArray.Length - 1))
            {
                continueButton.gameObject.SetActive(true);
                yield return new WaitUntil(() => hasContinued);
                yield break;
            }

            nextPlayerButton.gameObject.SetActive(true);

            yield return new WaitUntil(() => hasTakenPic);
            frontCamera.RetakePictureButton();
        }

        yield return null;
    }

    public void CountDown()
    {
        if (countdownNumber == 1)
        {
            countdownNumber = 5;
            countdownNumberText.text = countdownNumber.ToString();
            return;
        }
        countdownNumber--;
        countdownNumberText.text = countdownNumber.ToString();
        SFXLibrary.Instance.Play(2);
    }

    public void ShowPlayersList()
    {
        playersListPanel.SetActive(true);
        frontCamera.cameraPreview = null;
        frontCamera.webcamTexture.Stop();
        frontCamera.webcamTexture = null;

    }

    public void NextPlayer()
    {
        hasTakenPic = true;
    }

    public void Continue()
    {
        hasContinued = true;
        ShowPlayersList();
    }

    private IEnumerator InitialTextDOTween()
    {
        RectTransform textRT = initialPopupText.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => initialPopupText.SetActive(true));
        textSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);

        yield return null;
    }
}
