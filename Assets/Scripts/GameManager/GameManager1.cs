using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1
{
    private static List<Player> tempPlayersList;
    public static int currentRound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator PlayerTurnLoop()
    {
        tempPlayersList = PlayerData.playersArray.ToList();
        tempPlayersList = tempPlayersList.OrderBy(p => Random.value).ToList();

        foreach (Player i in tempPlayersList)
        {
            i.HasPlayed = false;
        }

        for (int i = 0; i < tempPlayersList.Count; i++)
        {
            PlayerData.currentPlayerTurn = tempPlayersList[i];
            Debug.Log("Player turn: " + PlayerData.currentPlayerTurn.PlayerName);

            yield return new WaitUntil(() => PlayerData.currentPlayerTurn.HasPlayed == true);

            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Wheel");

            Debug.Log("Player finished turn: " + PlayerData.currentPlayerTurn.PlayerName);
        }

        yield break;
    }

    public static IEnumerator RoundsLoop()
    {
        for (currentRound = 1; currentRound <= PlayerData.numberOfRounds; currentRound++)
        {
            Debug.Log("Round " + currentRound);
            yield return PlayerTurnLoop();
        }

        Debug.Log("All rounds finished!");
        SceneManager.LoadScene("LeaderBoard");
    }

    public static void EndRound()
    {
        PlayerData.currentPlayerTurn.HasPlayed = true;
        SceneManager.LoadScene("Wheel");
    }
}
