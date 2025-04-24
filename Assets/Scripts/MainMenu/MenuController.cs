using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel; // Panel showing credits
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Update()
    {
        if (creditsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCredits();
            }
        }

        if (playPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePlay();   
            }
        }

        if (settingsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePlay();
            }
        }
    }

    public void ConfirmGame()
    {
        StartCoroutine(PlayerData.AssignPlayers());
        CoroutineHost.instance.Run(GameManager1.RoundsLoop());
        SceneManager.LoadScene("Wheel"); // Ersätt Scene 1 med den första scenen
    }

    public void ShowPlay()
    {
        playPanel.SetActive(true);
    }

    public void ClosePlay()
    {
        playPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
