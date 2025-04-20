using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    List<Player> playersList;
    [SerializeField] private GameObject entryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));
        }
        playersList = SortByScore(PlayerData.playersArray);
        InitializeLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeLeaderBoard()
    {
        GameObject currentEntry;

        foreach (var player in playersList)
        {
            currentEntry = null;
            currentEntry = Instantiate(entryPrefab);
            currentEntry.transform.SetParent(gameObject.transform);
            currentEntry.GetComponent<LeaderBoardEntry>().Player = player;
            currentEntry.GetComponent<LeaderBoardEntry>().LoadEntry();
        }
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
}
