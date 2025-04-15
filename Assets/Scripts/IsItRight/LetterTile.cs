using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    public TextMeshProUGUI letterText;

    public string displayedLetter;
    public string correctLetter;
    public string letter; // ← Add this if needed for GameManager
    public RectTransform letterTextTransform;
    public Transform contentTransform; // används för flip
    public Transform bounceTarget;     // används för bounce (hela knappen)


    public bool isCorrect;
    private bool hasBeenPressed = false;

    public void Setup(string shown, string correct, bool isCorrectLetter, bool startsCorrect)
    {
        StopAllCoroutines(); // 🛑 Stop lingering fades or shakes

        displayedLetter = shown;
        correctLetter = correct;
        isCorrect = isCorrectLetter;
        hasBeenPressed = false;

        letterText.text = shown;

        // ✅ Always reset visuals
        Image img = GetComponent<Image>();
        img.color = Color.white;

        GetComponent<Button>().interactable = true;
    }


    public static string FlipLetter(string c)
    {
        Dictionary<char, char> flipMap = new()
    {
        { 'a', 'ɐ' }, { 'b', 'q' }, { 'c', 'ɔ' }, { 'd', 'p' }, { 'e', 'ǝ' },
        { 'f', 'ɟ' }, { 'g', 'ƃ' }, { 'h', 'ɥ' }, { 'i', 'ᴉ' }, { 'j', 'ɾ' },
        { 'k', 'ʞ' }, { 'l', 'l' }, { 'm', 'ɯ' }, { 'n', 'u' }, { 'o', 'o' },
        { 'p', 'd' }, { 'q', 'b' }, { 'r', 'ɹ' }, { 's', 's' }, { 't', 'ʇ' },
        { 'u', 'n' }, { 'v', 'ʌ' }, { 'w', 'ʍ' }, { 'x', 'x' }, { 'y', 'ʎ' },
        { 'z', 'z' }
    };

        char ch = c.ToLower()[0];
        return flipMap.ContainsKey(ch) ? flipMap[ch].ToString() : c;
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
        displayedLetter = FlipLetter(correctLetter);
        letterText.text = displayedLetter;

        // ✅ Reset visuals
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
        hasBeenPressed = false;
    }


    private IEnumerator AnimateFlip()
    {
        float duration = 0.08f;
        float elapsed = 0f;
        Vector3 originalScale = contentTransform.localScale;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float scaleX = Mathf.Lerp(1f, 0f, progress);
            contentTransform.localScale = new Vector3(scaleX, 1f, 1f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        letterText.text = correctLetter;
        StartCoroutine(Bounce());


        // 🟢 Fördröj färgsättning tills efter flip-back är klar
        elapsed = 0f;
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float scaleX = Mathf.Lerp(0f, 1f, progress);
            contentTransform.localScale = new Vector3(scaleX, 1f, 1f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        GetComponent<Button>().interactable = false;

        if (isCorrect)
        {
            GetComponent<Image>().color = isItRight.Instance.victoryGreen;
            StartCoroutine(Bounce()); // 🟢 studsa på rätt bokstav
            isItRight.Instance.OnCorrectLetterTapped(this);
        }
        else
        {
            GetComponent<Image>().color = Color.red;
            isItRight.Instance.OnWrongLetterTapped(this);
        }


    }

    private IEnumerator AnimateShake()
    {
        Vector3 originalPos = transform.localPosition;
        float shakeAmount = 10f;
        float shakeDuration = 0.2f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeAmount;
            transform.localPosition = originalPos + new Vector3(offsetX, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;

        // Show red & disable
        GetComponent<Image>().color = Color.red;
        isItRight.Instance.OnWrongLetterTapped(this);
        GetComponent<Button>().interactable = false;
    }
    public IEnumerator ShowVictoryState(Color green)
    {
        yield return FadeToColor(green);
    }


    private IEnumerator FadeToColor(Color targetColor)
    {
        Image img = GetComponent<Image>();
        Color startColor = img.color;
        float duration = 0.3f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            img.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }
    public IEnumerator Bounce()
    {
        Vector3 originalScale = bounceTarget.localScale;
        Vector3 target = originalScale * 1.2f;
        float t = 0f;

        while (t < 0.1f)
        {
            t += Time.deltaTime;
            bounceTarget.localScale = Vector3.Lerp(originalScale, target, t / 0.1f);
            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            bounceTarget.localScale = Vector3.Lerp(target, originalScale, t / 0.1f);
            yield return null;
        }
        bounceTarget.localScale = Vector3.one;
    }
    public void ResetTile()
    {
        hasBeenPressed = false;
        StopAllCoroutines(); // cancel fades/shakes/etc.

        Image img = GetComponent<Image>();
        img.color = Color.white;

        GetComponent<Button>().interactable = true;

        // 🔁 Sätt tillbaka displayedLetter om den finns
        letterText.text = displayedLetter ?? correctLetter ?? "?";
    }
    public void SetVictoryColor(Color green)
    {
        StopAllCoroutines();
        GetComponent<Image>().color = green;

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


}


