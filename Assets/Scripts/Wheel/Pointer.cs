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
                nextGameText.text = "Game1 will begin now";
                StartCoroutine(Timer("X 3IsItRight"));
                break;

            case "Game2":
                nextGameText.text = "Game2 will begin now";
                StartCoroutine(Timer("X 6DontGetHit"));
                break;

            case "Game3":
                nextGameText.text = "Game3 will begin now";
                StartCoroutine(Timer("X 5PopTheBalloon"));
                break;
        }
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }

    IEnumerator Timer(string levelName)
    {
        yield return new WaitForSeconds(gameStartTimer);

        SceneManager.LoadScene(levelName);
    }
}
