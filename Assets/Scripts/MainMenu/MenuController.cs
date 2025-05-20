using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject wheelSettingsPanel;

    public string mainMenuSceneName = "HuvudMenu";

    private void Update()
    {
        if (creditsPanel != null && creditsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseCredits();
        if (playPanel != null && playPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            ClosePlay();
        if (settingsPanel != null && settingsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseSettings();
        if (optionsPanel != null && optionsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseOptions();
        if (wheelSettingsPanel != null && wheelSettingsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseWheelSettings();
    }

    public void ConfirmGame()
    {
        StartCoroutine(ConfirmGameRoutine());
    }

    private IEnumerator ConfirmGameRoutine()
    {
        Debug.Log("[MenuController] Assigning players...");
        yield return PlayerData.AssignPlayers();

        Debug.Log("[MenuController] Players assigned.");

        SceneTransition transition = GameObject.FindObjectOfType<SceneTransition>();
        if (transition != null)
        {
            transition.StartFadeOut("X 1Camera");
        }
        else
        {
            Debug.LogWarning("[MenuController] SceneTransition not found, loading scene directly.");
            SceneManager.LoadScene("X 1Camera");
        }
    }

    public void ShowPanelWithDelay(GameObject panel) => StartCoroutine(DelayedShowPanel(panel));
    private IEnumerator DelayedShowPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.2f);
        panel.SetActive(true);
    }

    public void ShowPlayWithDelay() => ShowPanelWithDelay(playPanel);
    public void ClosePlay() => playPanel.SetActive(false);
    public void ShowSettingsWithDelay() => ShowPanelWithDelay(settingsPanel);
    public void CloseSettings() => settingsPanel.SetActive(false);
    public void ShowOptionsWithDelay() => ShowPanelWithDelay(optionsPanel);
    public void CloseOptions() => optionsPanel.SetActive(false);
    public void ShowWheelSettingsWithDelay() => ShowPanelWithDelay(wheelSettingsPanel);
    public void CloseWheelSettings() => wheelSettingsPanel.SetActive(false);
    public void QuitGame() => Application.Quit();
    public void ShowCreditsWithDelay() => ShowPanelWithDelay(creditsPanel);
    public void CloseCredits() => creditsPanel.SetActive(false);
    public void ReturnToMenu() => SceneManager.LoadScene(mainMenuSceneName);
}
