using UnityEngine;

public class TargetCounterGameManager : MonoBehaviour
{
    [Tooltip("How many targets must be hit to win")]
    public int targetsToHit = 3;

    private int hitCount = 0;
    private bool gameEnded = false;

    public static TargetCounterGameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterHit()
    {
        if (gameEnded) return;

        hitCount++;

        Debug.Log($"🎯 Target hit! {hitCount}/{targetsToHit}");

        if (hitCount >= targetsToHit)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        Debug.Log("🏆 All targets hit! Point awarded.");
        FindObjectOfType<StarBurstDOTween>().TriggerBurst();
        PlayerData.currentPlayerTurn.AddScore(1);
        GameManager1.EndTurn();
    }
}
