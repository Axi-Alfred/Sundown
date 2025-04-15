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

    private int currentIconInt;

    
    
    public Player(string name, int iD, Sprite icon)
    {
        playerName = name;
        playerId = iD;
        playerIcon = icon;
        currentIconInt = iD;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    public void SetPlayerImage(Sprite image)
    {
        playerIcon = image;
    }

    public void ChangePlayerIcon()
    {
        playerIcon = PlayerData.iconDatabase.iconsArray[++currentIconInt % PlayerData.iconDatabase.iconsArray.Length];
    }

    public Sprite GetPlayerIcon()
    {
        return playerIcon;  
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

    public int GetPlayerScore()
    {
        return playerScore;
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
