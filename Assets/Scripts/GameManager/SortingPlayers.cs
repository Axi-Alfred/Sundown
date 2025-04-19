using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SortingPlayers
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<Player> SortByScore(Player[] players)
    {
        List<Player> playersCopy = players.ToList();

        playersCopy.Sort((a, b) => b.PlayerScore.CompareTo(a.PlayerScore));

        return playersCopy;
    }
}
