using UnityEngine;

public class DelayedPointAwarder : MonoBehaviour
{
    [Header("Scoring Settings")]
    [Tooltip("How many seconds must pass before awarding the point")]
    public float timeToAwardPoint = 5f;

    private bool hasAwardedPoint = false;

    void Start()
    {
        Invoke(nameof(AwardPoint), timeToAwardPoint);
    }

    private void AwardPoint()
    {
        if (hasAwardedPoint) return;

        hasAwardedPoint = true;
        Debug.Log("🏆 Delayed point awarded!");
        FindObjectOfType<StarBurstDOTween>().TriggerBurst();
        PlayerData.currentPlayerTurn.AddScore(1);
        GameManager1.EndTurn();
    }

    void OnDestroy()
    {
        // If the GameObject is destroyed (e.g., scene changes) before the point is awarded
        if (!hasAwardedPoint)
        {
            Debug.Log("❌ Scene changed or object destroyed before point was awarded. No score given.");
            CancelInvoke(nameof(AwardPoint));
        }
    }
}
