using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Image playerSprite;
    private bool wheelHasSpinned;

    [SerializeField] private float gameStartTimer = 2.5f;

    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;

        playerText.text = "Now spinning: " + PlayerManager.Instance.currentPlayerTurn.PlayerName;
        roundText.text = GameManager1.currentRound == PlayerManager.Instance.numberOfRounds
            ? "Round " + GameManager1.currentRound + ", Final Round"
            : "Round " + GameManager1.currentRound;

        playerSprite.sprite = PlayerManager.Instance.currentPlayerTurn.PlayerIcon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ScreenShake.instance.TriggerShake();

        string tag = other.tag;
        StartCoroutine(Timer(tag));
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }

    IEnumerator Timer(string tag)
    {
        yield return new WaitForSeconds(gameStartTimer);

        string sceneName = SpinWheel.GetSceneForTag(tag);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[Pointer] Scene name for tag '{tag}' not found!");
            yield break;
        }

        var transition = FindObjectOfType<SceneTransition>();
        if (transition != null)
            transition.StartFadeOut(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}
