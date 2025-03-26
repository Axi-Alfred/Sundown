using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string playerName;
    private List<GameObject> handOfCards;

    // Start is called before the first frame update
    void Start()
    {
        playerName = "Player " + 1; //the number of the amount of current players for the future instead of 1
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
