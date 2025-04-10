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


    public bool isCorrect;
    private bool hasBeenPressed = false;

    public void Setup(string shown, string correct, bool isCorrectLetter, bool startsCorrect)

    {
        displayedLetter = shown;
        correctLetter = correct;
        isCorrect = isCorrectLetter;

        letterText.text = shown;

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
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
        hasBeenPressed = false;
    }
    private IEnumerator AnimateFlip()
    {
        float duration = 0.08f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        // 🔁 Flip out (scale X → 0)
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float scaleX = Mathf.Lerp(1f, 0f, progress);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ⚡ Swap text
        letterText.text = correctLetter;

        // ✅ Color based on correctness
        if (isCorrect)
        {
            GetComponent<Image>().color = Color.green;
            isItRight.Instance.OnCorrectLetterTapped(this);
        }
        else
        {
            GetComponent<Image>().color = Color.red;
            isItRight.Instance.OnWrongLetterTapped(this);
        }

        // 🔁 Flip back (scale X → 1)
        elapsed = 0f;
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float scaleX = Mathf.Lerp(0f, 1f, progress);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        GetComponent<Button>().interactable = false;
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
    public IEnumerator ShowVictoryState(Color finalColor)
    {
        GetComponent<Image>().color = Color.white; // optional reset
        yield return new WaitForSeconds(0.05f);    // optional delay
        GetComponent<Image>().color = finalColor;
    }




}


