using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewLetter : MonoBehaviour
{
    // Script för att kontrollera utseendet på varje bokstav/tile

    [SerializeField] private Transform bounceTarget;

    public TextMeshProUGUI letterText; // Visar bokstaven
    public RectTransform letterTextTransform;
    public Transform contentTransform; // För att rotera bokstaven

    public string displayedLetter; // vad som visas för bokstaven
    public string correctLetter; // vad bokstaven egentligen innehåller
    public bool isCorrect;

    private bool hasBeenPressed = false;

    public void Setup(string shown, string correct, bool isCorrectLetter, bool startsCorrect)
    {
        StopAllCoroutines();

        displayedLetter = shown;
        correctLetter = correct;
        isCorrect = isCorrectLetter;
        hasBeenPressed = false;

        letterText.text = shown;
        GetComponent<Image>().color = Color.white;

        Button button = GetComponent<Button>();
        button.interactable = !string.IsNullOrEmpty(shown);

        contentTransform.localRotation = startsCorrect ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
    }

    public void OnClick()
    {
        if (hasBeenPressed) return;
        hasBeenPressed = true;

        if (isCorrect)
            StartCoroutine(AnimateFlip());
        else
            StartCoroutine(AnimateShake());
    }

    public void ForceFlip() // Forcerar flip om vi inte har tillräckligt med inkorrekta bokstäver
    {
        isCorrect = true;
        displayedLetter = correctLetter;
        letterText.text = correctLetter;

        contentTransform.localRotation = Quaternion.Euler(0, 0, 180);

        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
        hasBeenPressed = false;
    }
    private IEnumerator AnimateFlip() // Flippar bokstaven tillbaka till normalt
    {
        contentTransform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast)
                        .SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(0.2f);

        Transform target = bounceTarget != null ? bounceTarget : transform;
        target.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        GetComponent<Image>().DOColor(isItRight.Instance.victoryGreen, 0.3f);

        GetComponent<Button>().interactable = false;
        isItRight.Instance.OnCorrectLetterTapped(this);
    }

    public IEnumerator AnimateShake() // Om spelaren trycker på fel ord
{
    Vector3 originalPos = transform.localPosition;

    transform.DOLocalMoveX(originalPos.x + 10f, 0.05f)
             .SetLoops(4, LoopType.Yoyo)
             .OnComplete(() =>
             {
                 transform.localPosition = originalPos;

                 GetComponent<Image>().DOColor(Color.red, 0.2f);
                 GetComponent<Button>().interactable = false;

                 isItRight.Instance.OnWrongLetterTapped(this);
             });

    yield return new WaitForSeconds(0.25f);
}

    public IEnumerator ShowVictoryState(Color green) // Byter färg på bakgrunderna till bokstäverna
    {
        yield return FadeToColor(green);
    }
    public IEnumerator FadeToColor(Color targetColor)
    {
        GetComponent<Image>().DOColor(targetColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator Bounce() // Studsar ordet uppåt
    {
        if (bounceTarget == null) yield break;

        bounceTarget.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        yield return new WaitForSeconds(0.3f);
    }

    public void ResetTile() // Reset och pausar alla animationer som pågår
    {
        hasBeenPressed = false;
        StopAllCoroutines();

        Image img = GetComponent<Image>();
        img.DOColor(Color.white, 0.2f);

        GetComponent<Button>().interactable = true;

        letterText.text = displayedLetter ?? correctLetter ?? "?"; // Visar ? om inget finns. ?? betyder använd om inte är null
    }
    public void SetVictoryColor(Color green)
    {
        StopAllCoroutines();
        GetComponent<Image>().DOColor(green, 0.3f);

        if (letterText == null)
        {
            Debug.LogError($"❌ letterText is NULL on tile {name}!");
        }
        else
        {
            letterText.text = correctLetter ?? "?";
            Debug.Log($"✅ SetVictoryColor: {name} → '{letterText.text}'");
        }

        GetComponent<Button>().interactable = false;
    }
    public IEnumerator VictoryJump(float delay) // Orden studsar upp och ned och visar rätt ord
    {
        yield return new WaitForSeconds(delay);

        Transform target = bounceTarget != null ? bounceTarget : transform;
        Vector3 originalPos = target.localPosition;


        if (letterText != null)
            letterText.text = correctLetter;

        float jumpHeight = 40f;
        float jumpTime = 0.2f;

        target.DOLocalMoveY(originalPos.y + jumpHeight, jumpTime)
              .SetEase(Ease.OutQuad)
              .OnComplete(() =>
              {
                  target.DOLocalMoveY(originalPos.y, jumpTime)
                        .SetEase(Ease.InQuad);
              });
    }


}


