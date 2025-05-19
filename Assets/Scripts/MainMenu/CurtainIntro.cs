using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurtainIntro : MonoBehaviour
{
    public Image curtainImage;
    public Sprite curtainClosed;

    public GameObject clownObject;              // UI clown
    public GameObject mainMenuPanel;            // Panel med menyinnehåll
    private CanvasGroup menuCanvasGroup;        // För att göra fade in

    private void Start()
    {
        // Hämta CanvasGroup från menypanelen
        menuCanvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
        StartCoroutine(PlayCurtainIntro());
    }

    IEnumerator PlayCurtainIntro()
    {
        // Visa stängda draperier
        curtainImage.sprite = curtainClosed;
        curtainImage.color = new Color(1, 1, 1, 1);
        curtainImage.gameObject.SetActive(true);

        // Dölj meny & clown från början
        mainMenuPanel.SetActive(false);
        clownObject.SetActive(false);
        menuCanvasGroup.alpha = 0;

        // Vänta 2 sekunder
        yield return new WaitForSeconds(2f);

        // Visa clown och börja fallet
        clownObject.SetActive(true);
        RectTransform clownRect = clownObject.GetComponent<RectTransform>();

        Vector2 startPos = new Vector2(clownRect.anchoredPosition.x, 1800f);
        Vector2 endPos = new Vector2(clownRect.anchoredPosition.x, 1000f);
        float duration = 4f;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            clownRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Ta bort clown och draperi
        clownObject.SetActive(false);
        curtainImage.gameObject.SetActive(false);

        // Visa menyn och starta fade-in
        mainMenuPanel.SetActive(true);
        StartCoroutine(FadeInMenu());
    }

    IEnumerator FadeInMenu()
    {
        float t = 0;
        float duration = 1.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            menuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // Slå på interaktivitet efter fade
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }
}

