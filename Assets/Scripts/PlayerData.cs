using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private int numberOfPlayers;

    private static int currenPlayerTurn;

    private static Player[] playersArray;

    // Start is called before the first frame update
    void Start()
    {
        playersArray = new Player[numberOfPlayers];

        for (int i = 0; i < playersArray.Length; i++)
        {
            playersArray[i] = new Player("Player " + i + 1, i);
        }

        print(playersArray);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
