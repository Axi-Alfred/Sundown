using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class LeaderBoardEntry : MonoBehaviour
{
    //Det här är scriptet i prefaben som spawnar en gång per spelare i leaderboarden
    public Player Player { get; set; }
    public int Position { get; set; }

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private Image playerIcon;
    [SerializeField] private GameObject crown;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadEntry()
    {
        playerName.text = Player.PlayerName;
        playerScore.text = Player.PlayerScore.ToString();
        playerIcon.sprite = Player.PlayerIcon;
    }

    public void GiveCrown()
    {
        RectTransform textRT = crown.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one * 0.2f;

        Sequence textSequence = DOTween.Sequence();
        textSequence.AppendCallback(() => crown.SetActive(true));
        textSequence.Append(textRT.DOScale(1.1f, 0.4f).SetEase(Ease.OutBack));
        textSequence.Append(textRT.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        textSequence.AppendInterval(1.5f);
    }
}
