using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void NextTurn()
    {
        List<Player> tempPlayers = new List<Player>(PlayerData.playersArray);


        int placeHolderTurn = Random.Range(0, tempPlayers.Count);
        PlayerData.currentPlayerTurn = tempPlayers[placeHolderTurn];
        tempPlayers.RemoveAt(placeHolderTurn);

        //while (placeHolderTurn =! )
        //Maybe add a boolean named alreadyPlayed??
        
    }

    //public bool LoopPerRound()
  

    public void RoundsLoop()
    {

    }
}
