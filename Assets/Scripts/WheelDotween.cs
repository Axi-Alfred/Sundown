using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class WheelDotween : MonoBehaviour
{
    [SerializeField] private GameObject texts;
    [SerializeField] private SpinWheel spinWheel;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image playerIcon;

    [SerializeField] private TMP_Text spinText;

    [SerializeField] private GameObject playersInstructionsObject;

    private void Awake()
    {
        
        spinWheel.enabled = false;
        texts.gameObject.SetActive(false);  
    }
    // Start is called before the first frame update
    void Start()
    {
        playerIcon.sprite = PlayerData.currentPlayerTurn.PlayerIcon;
        playerName.text = PlayerData.currentPlayerTurn.PlayerName;
        playersInstructionsObject.SetActive(true);
        StartCoroutine(SceneInitialization());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SceneInitialization()
    {
        yield return new WaitForSeconds(2f);

        playerName.gameObject.GetComponent<RectTransform>().DOAnchorPosX(0, 0.75f);
        spinText.gameObject.GetComponent<RectTransform>().DOAnchorPosX(0, 0.75f);

        yield return new WaitForSeconds(3.5f);

        playerName.gameObject.GetComponent<RectTransform>().DOAnchorPosX(-1250, 0.75f);
        spinText.gameObject.GetComponent<RectTransform>().DOAnchorPosX(1250, 0.75f);

        yield return new WaitForSeconds(1f);

        playersInstructionsObject.SetActive(false);
        texts.gameObject.SetActive(true);
        spinWheel.enabled = true;
    }
}
