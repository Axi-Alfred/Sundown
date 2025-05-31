using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject playPanel;

    public string mainMenuSceneName = "HuvudMenu";

    private void Update()
    {
        if (creditsPanel != null && creditsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseCredits();
        if (playPanel != null && playPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            ClosePlay();
    }

    public void ConfirmGame()
    {
        StartCoroutine(ConfirmGameRoutine());
    }

    private IEnumerator ConfirmGameRoutine()
    {
        Debug.Log("[MenuController] Assigning players...");
        yield return PlayerData.AssignPlayers();
        CoroutineHost.instance.Run(GameManager1.RoundsLoop());

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
    public void QuitGame() => Application.Quit();
    public void ShowCreditsWithDelay() => ShowPanelWithDelay(creditsPanel);
    public void CloseCredits() => creditsPanel.SetActive(false);
    public void ReturnToMenu(GameObject button)
    {
        UIButtonAnimator animator = button.GetComponent<UIButtonAnimator>();

        if(animator != null)
        {
            animator.PlayClickAndThen(() =>
            {
                SceneManager.LoadScene(mainMenuSceneName);
            });
        }
    }
       
}
