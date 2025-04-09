using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1
{
    private static List<Player> tempPlayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void NextTurn()
    {
        int placeHolderTurn = Random.Range(0, tempPlayers.Count);
        PlayerData.currentPlayerTurn = tempPlayers[placeHolderTurn];
        tempPlayers.RemoveAt(placeHolderTurn);        
    }

    public bool PlayerTurnLoop()
    {
        tempPlayers = new List<Player>(PlayerData.playersArray);

        foreach (Player i in tempPlayers)
        {
            i.SetHasPlayed(false);
        }

        for (int i = 0; i < PlayerData.playersArray.Length;)
        {
            NextTurn();
            if (PlayerData.currentPlayerTurn.HasPlayed())
            {
                i++;
            }
        }
        return true;
    }
  
    public void RoundsLoop()
    {
        for (int i = 0; i < PlayerData.numberOfRounds;)
        {
            if (PlayerTurnLoop())
            {
                i++;
            }
        }
    }
}
