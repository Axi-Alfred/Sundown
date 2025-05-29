using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewLetter : MonoBehaviour
{
    public TextMeshProUGUI letterText;

    public string displayedLetter;
    public string correctLetter;
    public string letter; // ← Add this if needed for GameManager
    public RectTransform letterTextTransform;
    public Transform contentTransform; // används för flip
    [SerializeField] private GameObject dissolveFX;
    [SerializeField] private Transform bounceTarget;




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

        // Set interaction based on if letter is empty or not
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

    public void ForceFlip()
    {
        isCorrect = true;
        displayedLetter = correctLetter;
        letterText.text = correctLetter;

        contentTransform.localRotation = Quaternion.Euler(0, 0, 180);

        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
        hasBeenPressed = false;
    }
    private IEnumerator AnimateFlip()
    {
        contentTransform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast)
                        .SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(0.2f);

        // ✅ FX
        if (dissolveFX != null)
        {
            GameObject fx = Instantiate(dissolveFX, transform.position, Quaternion.identity);
            fx.transform.localScale = Vector3.one * 0.5f;
        }

        // ✅ Bounce + color
        Transform target = bounceTarget != null ? bounceTarget : transform;
        target.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        GetComponent<Image>().DOColor(isItRight.Instance.victoryGreen, 0.3f);

        // Disable & notify
        GetComponent<Button>().interactable = false;
        isItRight.Instance.OnCorrectLetterTapped(this);
    }

    public IEnumerator AnimateShake()
{
    Vector3 originalPos = transform.localPosition;

    // Use DOTween shake (X only)
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

    public IEnumerator ShowVictoryState(Color green)
    {
        yield return FadeToColor(green);
    }
    public IEnumerator FadeToColor(Color targetColor)
    {
        GetComponent<Image>().DOColor(targetColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator Bounce()
    {
        if (bounceTarget == null) yield break;

        bounceTarget.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        yield return new WaitForSeconds(0.3f);
    }

    public void ResetTile()
    {
        hasBeenPressed = false;
        StopAllCoroutines(); // cancel fades/shakes/etc.

        Image img = GetComponent<Image>();
        img.DOColor(Color.white, 0.2f);

        GetComponent<Button>().interactable = true;

        // 🔁 Sätt tillbaka displayedLetter om den finns
        letterText.text = displayedLetter ?? correctLetter ?? "?";
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
    public IEnumerator VictoryJump(float delay)
    {
        yield return new WaitForSeconds(delay);

        Transform target = bounceTarget != null ? bounceTarget : transform;
        Vector3 originalPos = target.localPosition;

        // ✅ Don't touch color
        // ✅ Optional: still show correct letter if needed
        if (letterText != null)
            letterText.text = correctLetter;

        // ✅ Punch upward
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


