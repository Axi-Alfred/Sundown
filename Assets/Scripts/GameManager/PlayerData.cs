using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static bool playersHaveBeenAssigned;

    [SerializeField] private int numberOfPlayers;

    public static IconDatabase iconDatabase;

    public static int numberOfRounds = 5;

    public static Player currentPlayerTurn;

    public static Player[] playersArray;

    private int maxNumberOfPlayer = 4;
    private int minNumberOfPlayer = 1;



    // Start is called before the first frame update
    void Start()
    {
        iconDatabase = Resources.Load<IconDatabase>("GlobalIcons");

        if (numberOfPlayers > maxNumberOfPlayer) numberOfPlayers = maxNumberOfPlayer;
        if (numberOfPlayers < minNumberOfPlayer) numberOfPlayers += minNumberOfPlayer;

        playersArray = new Player[numberOfPlayers];

        for (int i = 0; i < playersArray.Length; i++)
        {
            playersArray[i] = new Player("Player " + (i+1), i, iconDatabase.iconsArray[i]);
        }

        StartCoroutine(CheckIfPlayersHaveBeenAssigned());

        /* foreach (Player player in playersArray)
        {
            print(player);
        } */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckIfPlayersHaveBeenAssigned()
    {
        yield return new WaitUntil(() => PlayerData.playersArray != null && PlayerData.playersArray.All(p => p != null));

        playersHaveBeenAssigned = true;
    }
}
