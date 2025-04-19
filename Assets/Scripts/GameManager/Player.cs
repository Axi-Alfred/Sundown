using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int PlayerId { get; private set; } //Tjena det e Sadra som har skrivit de h�r, om n�gon ser det h�r och undrar vad det �r f�r n�got fr�ga g�rna s� kan jag f�rklara :D
    public int PlayerScore { get; private set; } = 0;
    public bool HasPlayed { get; set; }
    public int CurrentIconInt { get; private set; }

    private string playerName;
    public string PlayerName {  get { return playerName; } set { playerName = value; }}

    private Sprite playerIcon;
    public Sprite PlayerIcon { get { return playerIcon; } private set { playerIcon = value; }}


    public Player(string name, int iD, Sprite icon)
    {
        playerName = name;
        PlayerId = iD;
        playerIcon = icon;
        CurrentIconInt = iD;
    }
    public void ChangePlayerIcon()
    {
        playerIcon = PlayerData.iconDatabase.iconsArray[++CurrentIconInt % PlayerData.iconDatabase.iconsArray.Length];
    }

    public void AddScore(int score)
    {
        PlayerScore += score;
    }

    override
    public string ToString()
    {
        return "Player name: " + PlayerName + ", " + "Player iD: " + PlayerId;
    }
}
