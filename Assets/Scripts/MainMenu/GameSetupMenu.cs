using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSetupMenu : MonoBehaviour
{
    public TMP_Dropdown playerDropdown; // Dra in dropdown för antal spelare
    public TMP_Dropdown roundDropdown; // Dra in dropdown för antal rundor
    public GameObject playerSetupPanel; // Dra in panelen som ska visas efter

    // Variabler som lagrar valen
    public static int numberOfPlayers;
    public static int numberOfRounds;

    // Körs när användaren klickar på Confirm
    public void OnConfirmSettings()
    {
        numberOfPlayers = playerDropdown.value + 1; // Justera beroende på dropdownens startvärde

        switch (roundDropdown.value)
        {
            case 0:
                numberOfRounds = 4;
                break;

            case 1:
                numberOfRounds = 6;
                break;

            case 2:
                numberOfRounds = 8;
                break;

            case 3:
                numberOfRounds = 10;
                break;
        }

        Debug.Log("Antal spelare: " + numberOfPlayers);
        Debug.Log("Antal rundor: " + numberOfRounds);

        // Visa nästa panel
        playerSetupPanel.SetActive(true);
        gameObject.SetActive(false); // Stänger nuvarande panel
    }
}
