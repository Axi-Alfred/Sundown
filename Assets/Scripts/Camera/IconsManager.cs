using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Random = UnityEngine.Random;

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

    private string[] playerNumbersStrings = new string[4] {"one", "two", "three", "four"};

    // Start is called before the first frame update
    void Start()
    {
        //This code is temporary and used only for testing
        PlayerData.numberOfPlayers = 4;
        StartCoroutine(PlayerData.AssignPlayers());

        frontCamera = GetComponent<FrontCamera>();    
        animator = GetComponent<Animator>();


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
    }

    public void ShowPlayersList()
    {

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
}
