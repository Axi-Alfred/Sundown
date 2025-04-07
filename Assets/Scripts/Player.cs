using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string playerName;
    private int playerId;
    private int playerScore;

    private bool hasPlayed;

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

    public void AddScore()
    {
        playerScore++;
    }

    public bool HasPlayed()
    {
        return hasPlayed;
    }

    override
    public string ToString()
    {
        return "Player name: " + playerName + ", " + "Player iD: " + playerId;
    }
}
