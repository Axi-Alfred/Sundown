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

    [SerializeField] private int currentPlayerIndex;

    private static int instanceCount;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(First());
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            image.sprite = player.PlayerIcon;
        }
    }

    public void ChangeIcon()
    {
        player.ChangePlayerIcon();
    }

    public void ChangeName()
    {
        player.PlayerName = inputField.text;
        Debug.Log(player.PlayerName);
    }

    IEnumerator First()
    {
        yield return new WaitUntil(() => PlayerData.playersHaveBeenAssigned == true);

        player = PlayerData.playersArray[currentPlayerIndex]; //TEMP
        inputField.text = player.PlayerName;
    }

    public Player CurrentPlayer()
    {
        return player;
    }

    private void OnEnable()
    {
        instanceCount++;
        Debug.Log(instanceCount);
    }
}

