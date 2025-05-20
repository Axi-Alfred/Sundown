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
        tempPlayersList = PlayerManager.Instance.playersArray.ToList();
        tempPlayersList = tempPlayersList.OrderBy(p => Random.value).ToList();

        foreach (Player i in tempPlayersList)
        {
            i.HasPlayed = false;
        }

        for (int i = 0; i < tempPlayersList.Count; i++)
        {
            PlayerManager.Instance.currentPlayerTurn = tempPlayersList[i];
            Debug.Log("Player turn: " + PlayerManager.Instance.currentPlayerTurn.PlayerName);

            yield return new WaitUntil(() => PlayerManager.Instance.currentPlayerTurn.HasPlayed == true);
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Wheel");

            Debug.Log("Player finished turn: " + PlayerManager.Instance.currentPlayerTurn.PlayerName);
        }

        yield break;
    }

    public static IEnumerator RoundsLoop()
    {
        gameSpeedMultiplier = 1;

        for (currentRound = 1; currentRound <= PlayerManager.Instance.numberOfRounds; currentRound++)
        {
            newRoundHasBegun = true;
            gameSpeedMultiplier = Mathf.Min(1f + (currentRound * increasePercentage), 2f);
            yield return PlayerTurnLoop();
        }

        Debug.Log("All rounds finished!");
        SceneManager.LoadScene("LeaderBoard");
    }

    public static void EndTurn()
    {
        PlayerManager.Instance.currentPlayerTurn.HasPlayed = true;
        Time.timeScale = 1f;

        var sceneController = UnityEngine.Object.FindObjectOfType<ScenesController>();
        if (sceneController != null)
        {
            sceneController.EndGameAndFadeOut(); // ✅ uses nextSceneName
        }
        else
        {
            UnityEngine.Object.FindObjectOfType<SceneTransition>()?.StartFadeOut("Wheel");
        }
    }
}
