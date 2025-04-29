using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPrep : MonoBehaviour
{
    public static PlayerPrep instance;

    [SerializeField] private GameObject entryPrefab; //Prefaben för varje individuellt entry av spelare
    [SerializeField] private GameObject iconContainer;

    // Start is called before the first frame update
    void Start()
    {
        //Loopen är till att ta bort alla tidigare entries i leaderboarden innan man skapar de nya
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        InitializePlayersList();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (iconContainer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                iconContainer.SetActive(false);
            }
        }
    }

    private void InitializePlayersList()
    {
        GameObject currentEntry;

        foreach (var player in PlayerData.playersArray)
        {
            currentEntry = null;
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(gameObject.transform);
            currentEntry.GetComponent<PlayerEntry>().Player = player;
            currentEntry.GetComponent<PlayerEntry>().LoadEntry();
        }
    }

    public void ShowPicture(Player player)
    {
        iconContainer.SetActive(true);
        Image image = iconContainer.GetComponent<Image>();
        image.sprite = player.PlayerIcon;
        float imgWidth = image.sprite.rect.width;
        float imgHeight = image.sprite.rect.height;
        RectTransform rt = image.GetComponent<RectTransform>();
        float aspectRatio = imgWidth / imgHeight;
        float newHeight = rt.sizeDelta.y;
        float newWidth = newHeight * aspectRatio;
        rt.sizeDelta = new Vector2(newWidth, newHeight);

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Wheel");
    }
}

