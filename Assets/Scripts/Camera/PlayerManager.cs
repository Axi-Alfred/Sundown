using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int numberOfPlayers;
    public int numberOfRounds;
    public Player currentPlayerTurn;
    public Player[] playersArray;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ✅ Call this after each picture is taken to see if all players are done
    public bool AllPlayersCaptured()
    {
        if (playersArray == null || playersArray.Length == 0)
            return false;

        foreach (Player player in playersArray)
        {
            if (player == null || player.PlayerIcon == null)
                return false;
        }

        return true;
    }
}
