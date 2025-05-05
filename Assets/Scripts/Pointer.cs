using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Pointer : MonoBehaviour
{
    [SerializeField] private TMP_Text nextGameText;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Image playerSprite;
    private bool wheelHasSpinned;

    [SerializeField] private float gameStartTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        nextGameText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        playerText.text = "Now spinning: " + PlayerData.currentPlayerTurn.PlayerName;
        roundText.text = GameManager1.currentRound == PlayerData.numberOfRounds ? "Round " + GameManager1.currentRound + ", Final Round" : "Round " + GameManager1.currentRound;
        playerSprite.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!nextGameText.gameObject.activeSelf) nextGameText.gameObject.SetActive(true);
        switch (other.tag)
        {
            case "Game1":
                nextGameText.text = "Is It Right? will begin now";
                StartCoroutine(Timer("X 3IsItRight"));
                break;

            case "Game2":
                nextGameText.text = "Odd One Out will begin now";
                StartCoroutine(Timer("X 4OddOneOut - Done"));
                break;

            case "Game3":
                nextGameText.text = "Pop The Balloon will begin now";
                StartCoroutine(Timer("X 5PopTheBalloon"));
                break;

            case "Game4":
                nextGameText.text = "Juggle Mania will begin now";
                StartCoroutine(Timer("X 7JuggleMania"));
                break;

            case "Game5":
                nextGameText.text = "Throwing Pies will begin now";
                StartCoroutine(Timer("X 8ThrowingPies"));
                break;

            case "Game6":
                nextGameText.text = "Smacked Pig will begin now";
                StartCoroutine(Timer("X 9SmackedPig"));
                break;

            case "Game7":
                nextGameText.text = "Cotton Candy will begin now";
                StartCoroutine(Timer("X 10CottonCandy"));
                break;

            case "Game8":
                nextGameText.text = "Falling Gods will begin now";
                StartCoroutine(Timer("X 11FallingGods"));
                break;

            case "Game9":
                nextGameText.text = "Clown Elope will begin now";
                StartCoroutine(Timer("X 12ClownElope"));
                break;

            case "Game10":
                nextGameText.text = "Dunk Tank will begin now";
                StartCoroutine(Timer("X 13DunkTank"));
                break;

            case "Game11":
                nextGameText.text = "Ball Toss will begin now";
                StartCoroutine(Timer("X 14BallToss_Scene"));
                break;

            case "Game12":
                nextGameText.text = "Catch Hop will begin now";
                StartCoroutine(Timer("X 15CatchHop_Main"));
                break;

            case "RandomGame":
                nextGameText.text = "Random game will now begin";
                StartCoroutine(Timer(Random.Range(4, 9))); //De här siffrorna kommer ändras beroende på vilka scener vi har med och vad de har för index
                break;
        }
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }

    IEnumerator Timer(object level)
    {
        yield return new WaitForSeconds(gameStartTimer);

        SceneTransition.FadeOut(level);
    }
}
