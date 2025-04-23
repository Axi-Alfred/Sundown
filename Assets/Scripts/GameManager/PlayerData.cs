using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{
    public static bool playersHaveBeenAssigned;

    public static int numberOfPlayers;

    public static IconDatabase iconDatabase;

    public static int numberOfRounds;

    public static Player currentPlayerTurn;

    public static Player[] playersArray;

    private static int maxNumberOfPlayer = 4;
    private static int minNumberOfPlayer = 1;


    public static IEnumerator AssignPlayers()
    {
        iconDatabase = Resources.Load<IconDatabase>("GlobalIcons");

        if (numberOfPlayers > maxNumberOfPlayer) numberOfPlayers = maxNumberOfPlayer;
        if (numberOfPlayers < minNumberOfPlayer) numberOfPlayers += minNumberOfPlayer;

        playersArray = new Player[numberOfPlayers];

        for (int i = 0; i < playersArray.Length; i++)
        {
            playersArray[i] = new Player("Player " + (i + 1), i, iconDatabase.iconsArray[i]);
        }

        yield return new WaitUntil(() => PlayerData.playersArray != null && PlayerData.playersArray.All(p => p != null));

        playersHaveBeenAssigned = true;
    }
}
