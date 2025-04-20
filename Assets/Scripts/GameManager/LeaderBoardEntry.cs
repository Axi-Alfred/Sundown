using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LeaderBoardEntry : MonoBehaviour
{
    //Det här är scriptet i prefaben som spawnar en gång per spelare i leaderboarden
    public Player Player { get; set; }

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private Image playerIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEntry()
    {
        playerName.text = Player.PlayerName;
        playerScore.text = Player.PlayerScore.ToString();
        playerIcon.sprite = Player.PlayerIcon;
    }
}
