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
        //Hantering av escape-knapp för att stänga paneler
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
        SceneTransition.FadeOut("X 1Camera");
        //SceneManager.LoadScene("X 1Camera"); // Ersätt Scene 1 med den första scenen
    }

    // Generell metod för att visa paneler med en fördröjning
    public void ShowPanelWithDelay(GameObject panel)
    {
        StartCoroutine(DelayedShowPanel(panel));
    }

    // Coroutine för att visa panel efter en fördröjning
    private IEnumerator DelayedShowPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.2f); // Justera fördröjning om animationen tar längre tid
        panel.SetActive(true);
    }

    // Funktioner för att visa och stänga paneler
    public void ShowPlayWithDelay()
    {
        ShowPanelWithDelay(playPanel);
    }

    public void ClosePlay()
    {
        playPanel.SetActive(false);
    }

    public void ShowSettingsWithDelay()
    {
        ShowPanelWithDelay(settingsPanel);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ShowOptionsWithDelay()
    {
        ShowPanelWithDelay(optionsPanel);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ShowWheelSettingsWithDelay()
    {
        ShowPanelWithDelay(wheelSettingsPanel);
    }

    public void CloseWheelSettings()
    {
        wheelSettingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCreditsWithDelay()
    {
        ShowPanelWithDelay(creditsPanel);
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
