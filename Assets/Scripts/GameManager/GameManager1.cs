using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager1
{
    private static List<Player> tempPlayersList;
    public static int currentRound;

    public static float gameSpeedMultiplier;
    private static float increasePercentage = 0.15f;

    public static bool newRoundHasBegun;

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
        gameSpeedMultiplier = 1;

        for (currentRound = 1; currentRound <= PlayerData.numberOfRounds; currentRound++)
        {
            newRoundHasBegun = true; //Sets till false i WheelDotween när animationen har spelat klar
            gameSpeedMultiplier = Mathf.Min(1f + (currentRound * increasePercentage), 2f);
            yield return PlayerTurnLoop();
        }

        Debug.Log("All rounds finished!");
        SceneManager.LoadScene("LeaderBoard");
    }

    public static void EndTurn()
    {
        PlayerData.currentPlayerTurn.HasPlayed = true;
        UnityEngine.Object.FindObjectOfType<SceneTransition>()?.StartFadeOut("SceneName");
    }
}
