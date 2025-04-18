using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Pointer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Image playerSprite;
    private bool wheelHasSpinned;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        playerText.text = "Now spinning: " + PlayerData.currentPlayerTurn.GetPlayerName();
        roundText.text = "Round " + GameManager1.currentRound;
        playerSprite.sprite = PlayerData.currentPlayerTurn.GetPlayerIcon();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!text.gameObject.activeSelf) text.gameObject.SetActive(true);
        switch (other.tag)
        {
            case "Game1":
                text.text = "Game1 will begin now";
                break;

            case "Game2":
                text.text = "Game2 will begin now";
                break;

            case "Game3":
                text.text = "Game3 will begin now";
                break;
        }
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }
}
