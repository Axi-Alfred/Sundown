using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    public Player Player { get; set; }

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image playerIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEntry()
    {
        playerName.text = Player.PlayerName;
        playerIcon.sprite = Player.PlayerIcon;
    }

    public void ShowPlayer()
    {
        PlayerPrep.instance.ShowPicture(Player);
    }
}
