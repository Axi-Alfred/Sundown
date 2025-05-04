using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    List<Player> playersList;
    [SerializeField] private GameObject entryPrefab; //Prefaben för varje individuellt entry i leaderboarden

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
        InitializeLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeLeaderBoard()
    {
        playersList = SortByScore(PlayerData.playersArray);

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
