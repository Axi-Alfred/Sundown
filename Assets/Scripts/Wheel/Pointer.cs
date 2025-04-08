using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Pointer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text playerText;
    private bool wheelHasSpinned;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        GameManager1.RoundsLoop();
        print(PlayerData.currentPlayerTurn);
        playerText.text = "Now spinning: " + PlayerData.currentPlayerTurn.GetPlayerName();
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
