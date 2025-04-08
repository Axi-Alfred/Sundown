using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private static int numberOfPlayers;

    private int currenPlayerTurn;

    private Player[] playersArray;

    // Start is called before the first frame update
    void Start()
    {
        playersArray = new Player[numberOfPlayers];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
