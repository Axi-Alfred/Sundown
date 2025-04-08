using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string playerName;
    private int playerId;
    private int playerScore;

    private bool hasPlayed;

    private Sprite playerIcon;

    // Start is called before the first frame update
    
    public Player(string name, int iD, Sprite icon)
    {
        playerName = name;
        playerId = iD;
        playerIcon = icon;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    public void SetPlayerIcon(Sprite icon)
    {
        playerIcon = icon;
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

    public bool HasPlayed()
    {
        return hasPlayed;
    }

    public void SetHasPlayed(bool hasPlayed)
    {
        this.hasPlayed = hasPlayed;
    }

    override
    public string ToString()
    {
        return "Player name: " + playerName + ", " + "Player iD: " + playerId;
    }
}
