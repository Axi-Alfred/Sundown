using UnityEngine;

public class JuggleGameManager : MonoBehaviour
{
    public static JuggleGameManager Instance;

    [Header("Win Condition")]
    [Tooltip("How many successful juggles the player needs to win")]
    public int jugglesNeededToWin = 10;

    private int currentJuggles = 0;
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterJuggle()
    {
        if (gameEnded) return;

        currentJuggles++;

        Debug.Log($"🎯 Juggle {currentJuggles}/{jugglesNeededToWin}");

        if (currentJuggles >= jugglesNeededToWin)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        Debug.Log("👏 You win! Juggled enough.");
        PlayerManager.Instance.currentPlayerTurn.AddScore(1);
        GameManager1.EndTurn();
    }
}
