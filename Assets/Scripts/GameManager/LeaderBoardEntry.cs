using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderBoardEntry : MonoBehaviour
{
    // Assigned externally during instantiation
    public Player Player { get; set; }
    public int Position { get; set; }

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image crown;

    void Start()
    {
        // Nothing needed here right now
    }

    void Update()
    {
        // Optional: You can remove this if not used
    }

    public void LoadEntry()
    {
        if (Player == null)
        {
            Debug.LogWarning("[LeaderBoardEntry] No Player data assigned!");
            return;
        }

        playerName.text = Player.PlayerName;
        playerScore.text = Player.PlayerScore.ToString();
        playerIcon.sprite = Player.PlayerIcon;
    }

    public void GiveCrown()
    {
        crown.enabled = true;
    }
}
