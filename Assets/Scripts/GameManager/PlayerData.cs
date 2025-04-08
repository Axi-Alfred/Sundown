using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private Sprite[] iconsArray;

    //public static int currentPlayerTurn = 0;

    public static int numberOfRounds = 5;

    public static Player currentPlayerTurn;

    public static Player[] playersArray;

    private int maxNumberOfPlayer = 6;
    private int minNumberOfPlayer = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (numberOfPlayers > maxNumberOfPlayer) numberOfPlayers = maxNumberOfPlayer;
        if (numberOfPlayers < minNumberOfPlayer) numberOfPlayers += minNumberOfPlayer;

        playersArray = new Player[numberOfPlayers];

        for (int i = 0; i < playersArray.Length; i++)
        {
            playersArray[i] = new Player("Player " + (i+1), i, iconsArray[i]);
        }

        foreach (Player player in playersArray)
        {
            print(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
