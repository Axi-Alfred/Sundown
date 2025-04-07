using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string playerName;

    private int playerId;

    private int playerScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string GetPlayerName()
    {
        return playerName;
    }

    private void SetPlayerName(string newName)
    {
        playerName = newName;
    }
}
