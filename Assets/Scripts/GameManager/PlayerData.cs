using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{
    public static bool playersHaveBeenAssigned;

    public static int numberOfPlayers
    {
        get => PlayerManager.Instance.numberOfPlayers;
        set => PlayerManager.Instance.numberOfPlayers = value;
    }

    public static int numberOfRounds
    {
        get => PlayerManager.Instance.numberOfRounds;
        set => PlayerManager.Instance.numberOfRounds = value;
    }

    public static Player currentPlayerTurn
    {
        get => PlayerManager.Instance.currentPlayerTurn;
        set => PlayerManager.Instance.currentPlayerTurn = value;
    }

    public static Player[] playersArray
    {
        get => PlayerManager.Instance.playersArray;
        set => PlayerManager.Instance.playersArray = value;
    }

    private static int maxNumberOfPlayer = 4;
    private static int minNumberOfPlayer = 1;

    public static IEnumerator AssignPlayers()
    {
        if (numberOfPlayers > maxNumberOfPlayer) numberOfPlayers = maxNumberOfPlayer;
        if (numberOfPlayers < minNumberOfPlayer) numberOfPlayers = minNumberOfPlayer;

        Player[] generatedPlayers = new Player[numberOfPlayers];

        for (int i = 0; i < generatedPlayers.Length; i++)
        {
            generatedPlayers[i] = new Player("Player " + (i + 1), i);
        }

        yield return new WaitUntil(() => generatedPlayers.All(p => p != null));

        playersArray = generatedPlayers;
        currentPlayerTurn = null;

        playersHaveBeenAssigned = true;
    }
}
