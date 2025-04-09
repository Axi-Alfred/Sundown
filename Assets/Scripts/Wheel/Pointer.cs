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
    private bool wheelHasSpinned;

    private bool playersHaveBeenAssigned;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(TestStart());
    }

    IEnumerator TestStart()
    {
        yield return new WaitUntil(() =>
        PlayerData.playersArray != null && PlayerData.playersArray.All(p => p != null));

        StartCoroutine(GameManager1.RoundsLoop());
        playersHaveBeenAssigned = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        if (playersHaveBeenAssigned)
        {
            playerText.text = "Now spinning: " + PlayerData.currentPlayerTurn.GetPlayerName();
            roundText.text = "Round " + GameManager1.currentRound;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
