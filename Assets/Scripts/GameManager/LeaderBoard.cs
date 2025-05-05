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

    [SerializeField] private Transform entrySpawnPoint;

    private void Awake()
    {
        //ONLY TEMPORARY FOR PLAY TESTING, WILL BE REMOVED SOON!!!
        PlayerData.numberOfPlayers = 4;
        StartCoroutine(PlayerData.AssignPlayers());
    }
    // Start is called before the first frame update
    void Start()
    {
        //Loopen är till att ta bort alla tidigare entries i leaderboarden innan man skapar de nya
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        StartCoroutine(IntroDOTween());
        InitializeLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeLeaderBoard()
    {
        List<Player> playersList = SortByScore(PlayerData.playersArray);

        GameObject currentEntry;
        int i = 1;

        foreach (var player in playersList)
        {
            currentEntry = null;
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(entrySpawnPoint);
            currentEntry.GetComponent<LeaderBoardEntry>().Player = player;
            currentEntry.GetComponent<LeaderBoardEntry>().Position = i;
            currentEntry.GetComponent<LeaderBoardEntry>().LoadEntry();
            entriesObjectsList.Add(currentEntry);
            i++;
        }

        entriesObjectsList[0].GetComponent<LeaderBoardEntry>().GiveCrown();
    }

    public static List<Player> SortByScore(Player[] players)
    {
        List<Player> playersCopy = players.ToList();

        playersCopy.Sort((a, b) => b.PlayerScore.CompareTo(a.PlayerScore));

        return playersCopy;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("HuvudMenu");
    }

    private IEnumerator IntroDOTween()
    {
        introText.enabled = false;

        yield return new WaitForSeconds(2);

        RectTransform textRT = introText.gameObject.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => introText.enabled = true);
        textSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);
        textSequence.Join(textRT.DOAnchorPosY(300, 0.6f).SetEase(Ease.OutQuad));

        yield return textSequence.WaitForCompletion();

    }
}
