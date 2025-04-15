using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerPrep : MonoBehaviour
{
    public Player player;

    [SerializeField] private Image image;

    [SerializeField] private TMP_InputField inputField;

    // Start is called before the first frame update
    private void Start()
    {
        player = PlayerData.playersArray[1]; //TEMP
        inputField.text = player.GetPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = player.GetPlayerIcon();
    }

    public void ChangeIcon()
    {
        player.ChangePlayerIcon();
    }
}
