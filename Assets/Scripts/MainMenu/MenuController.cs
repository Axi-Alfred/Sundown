using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel; // Panel showing credits
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject wheelSettingsPanel;

    //Det är jag Sadra som har skrivit den här bara för att testa scene transition, don worry about this :)
    //[SerializeField] private SceneTransition sceneTransition;

    public string mainMenuSceneName = "HuvudMenu";

    private void Update()
    {
        if (creditsPanel != null && creditsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCredits();
            }
        }

        if (playPanel != null && playPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePlay();   
            }
        }

        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSettings();
            }
        }

        if (optionsPanel != null && optionsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseOptions();
            }
        }

        if (wheelSettingsPanel != null && wheelSettingsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWheelSettings();
            }
        }
    }

    public void ConfirmGame()
    {
        StartCoroutine(PlayerData.AssignPlayers());
        CoroutineHost.instance.Run(GameManager1.RoundsLoop());
        FindObjectOfType<SceneTransition>()?.StartFadeOut("X 1Camera");
        //SceneManager.LoadScene("X 1Camera"); // Ersätt Scene 1 med den första scenen
    }

    public void ShowPlay()
    {
        playPanel.SetActive(true);
    }

    public void ClosePlay()
    {
        playPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ShowWheelSettings()
    {
        wheelSettingsPanel.SetActive(true);
    }

    public void CloseWheelSettings()
    {
        wheelSettingsPanel.SetActive(false);
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

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
