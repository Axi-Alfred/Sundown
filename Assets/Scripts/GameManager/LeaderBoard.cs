using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class LeaderBoard : MonoBehaviour
{
    List<GameObject> entriesObjectsList = new List<GameObject>();
    [SerializeField] private GameObject entryPrefab; //Prefaben för varje individuellt entry i leaderboarden
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text introText;
    [SerializeField] private GameObject returnToMenuButton;

    [SerializeField] private Transform entrySpawnPoint;

    [SerializeField] private VerticalLayoutGroup layoutGroupV;

    [SerializeField] private GameObject confettiParticles;

    //private float[] pitches = new float[4] {}; 

    // Start is called before the first frame update
    void Start()
    {
        confettiParticles.SetActive(false);

        if (PlayerData.numberOfPlayers < 3)
        {
            layoutGroupV.childAlignment = TextAnchor.UpperCenter;
            layoutGroupV.spacing = -600;
        }
        else
        {
            layoutGroupV.childAlignment = TextAnchor.MiddleCenter;
            layoutGroupV.spacing = 50;
        }

        //Loopen är till att ta bort alla tidigare entries i leaderboarden innan man skapar de nya
        returnToMenuButton.SetActive(false);
        entriesObjectsList.Clear();
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        StartCoroutine(IntroDOTween());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
    }

    private void InitializeLeaderBoard()
    {
        List<Player> playersList = SortByScore(PlayerData.playersArray);

        GameObject currentEntry;

        foreach (var player in playersList)
        {
            currentEntry = null;
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(entrySpawnPoint);
            currentEntry.GetComponent<LeaderBoardEntry>().Player = player;
            currentEntry.GetComponent<LeaderBoardEntry>().LoadEntry();
            entriesObjectsList.Add(currentEntry);
            currentEntry.transform.localScale = Vector3.zero;
        }
    }

    public static List<Player> SortByScore(Player[] players)
    {
        List<Player> playersCopy = players.ToList();

        playersCopy.Sort((a, b) => a.PlayerScore.CompareTo(b.PlayerScore));

        return playersCopy;
    }

    public void ReturnToMenu()
    {
        var transition = FindObjectOfType<SceneTransition>();
        if (transition != null)
        {
            transition.StartFadeOut("HuvudMenu");
        }
        else
        {
            SceneManager.LoadScene("HuvudMenu");
        }

        // ✅ Clear data for next game
        PlayerManager.Instance.playersArray = null;
        PlayerManager.Instance.currentPlayerTurn = null;
        PlayerManager.Instance.numberOfPlayers = 0;
        PlayerManager.Instance.numberOfRounds = 0;
    }

    private IEnumerator IntroDOTween()
    {
        introText.enabled = false;

        yield return new WaitForSeconds(1.5f);

        RectTransform textRT = introText.gameObject.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => introText.enabled = true);
        textSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);
        textSequence.Join(textRT.DOAnchorPosY(300, 0.6f).SetEase(Ease.OutQuad));

        yield return textSequence.WaitForCompletion();

        InitializeLeaderBoard();
        StartCoroutine(AnimateEntriesSequentially());
    }

    private IEnumerator AnimateEntriesSequentially()
    {
        float delayBetweenEntries = 0.3f;
        float entryAnimDuration = 0.6f;
        float lastEntryAnimDuration = 1.0f;

        VerticalLayoutGroup layoutGroup = entrySpawnPoint.GetComponent<VerticalLayoutGroup>();
        bool hasLayoutGroup = layoutGroup != null;

        for (int i = 0; i < entriesObjectsList.Count; i++)
        {
            GameObject entryObject = entriesObjectsList[i];
            RectTransform rt = entryObject.GetComponent<RectTransform>();

            Canvas.ForceUpdateCanvases();
            if (hasLayoutGroup) LayoutRebuilder.ForceRebuildLayoutImmediate(entrySpawnPoint as RectTransform);
            Vector2 finalPosition = rt.anchoredPosition;

            rt.anchoredPosition = new Vector2(finalPosition.x, finalPosition.y + 100f);

            Sequence entrySequence = DOTween.Sequence();

            if (i == entriesObjectsList.Count - 1)
            {
                entrySequence.Append(rt.DOScale(Vector3.one * 1.5f, lastEntryAnimDuration * 0.6f).SetEase(Ease.OutBack));
                entrySequence.Append(rt.DOScale(Vector3.one, lastEntryAnimDuration * 0.4f).SetEase(Ease.InOutQuad));
                entrySequence.AppendInterval(0.7f);
                entrySequence.Append(rt.DOAnchorPosY(finalPosition.y, lastEntryAnimDuration * 0.6f).SetEase(Ease.OutBounce));
            }
            else
            {
                entrySequence.Append(rt.DOScale(Vector3.one * 1.2f, entryAnimDuration * 0.4f).SetEase(Ease.OutBack));
                entrySequence.Append(rt.DOScale(Vector3.one, entryAnimDuration * 0.2f).SetEase(Ease.InOutQuad));
                entrySequence.AppendInterval(0.5f);
                entrySequence.Append(rt.DOAnchorPosY(finalPosition.y, entryAnimDuration * 0.4f).SetEase(Ease.OutBounce));
            }

            yield return entrySequence.WaitForCompletion();

            yield return new WaitForSeconds(delayBetweenEntries);
        }

        confettiParticles.SetActive(true);
        entriesObjectsList[entriesObjectsList.Count - 1].GetComponent<LeaderBoardEntry>().GiveCrown();

        yield return new WaitForSeconds(1.5f);

        returnToMenuButton.SetActive(true);
    }
}
