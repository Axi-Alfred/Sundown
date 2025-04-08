using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static bool PlayerTurnLoop()
    {
        tempPlayers = PlayerData.playersArray.ToList();
        //tempPlayers = tempPlayers.OrderBy(p => Random.value).ToList();

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
  
    public static void RoundsLoop()
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
