using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel; // Panel showing credits
    [SerializeField] private GameObject playPanel;
    
    public void ConfirmGame()
    {
        SceneManager.LoadScene("Scene 1"); // Ersätt Scene 1 med den första scenen
    }

    public void ShowPlay()
    {
        playPanel.SetActive(true);
    }

    public void ClosePlay()
    {
        playPanel.SetActive(false);
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
