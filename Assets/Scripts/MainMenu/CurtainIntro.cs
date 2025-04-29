using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurtainIntro : MonoBehaviour
{
    public Image curtainImage;
    public Sprite curtainClosed;
    public Sprite curtainHalfOpen;

    public GameObject mainMenuPanel; // Assign this to your actual menu panel

    private void Start()
    {
        StartCoroutine(PlayCurtainIntro());
    }

    IEnumerator PlayCurtainIntro()
    {
        curtainImage.sprite = curtainClosed;
        yield return new WaitForSeconds(2f);

        curtainImage.sprite = curtainHalfOpen;
        yield return new WaitForSeconds(1f);


        // Optionally, fade out the curtain or disable it
        yield return new WaitForSeconds(0.5f);

        // Show the menu
        mainMenuPanel.SetActive(true);
        curtainImage.gameObject.SetActive(false); // Hide curtain if needed
    }
}

