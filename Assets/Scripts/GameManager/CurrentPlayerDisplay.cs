using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CurrentPlayerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] public Image playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = PlayerData.currentPlayerTurn.PlayerName;
        playerSprite.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
    }
}
