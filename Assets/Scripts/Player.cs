using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string playerName;

    private int playerId;

    private int playerScore;

    // Start is called before the first frame update
    
    public Player(string name, int iD)
    {
        playerName = name;
        playerId = iD;
    }

    private string GetPlayerName()
    {
        return playerName;
    }

    private void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    private void AddScore()
    {
        playerScore++;
    }
}
