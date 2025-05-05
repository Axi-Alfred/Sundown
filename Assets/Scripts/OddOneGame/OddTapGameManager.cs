//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class OddPieGameManager : MonoBehaviour
//{
//    [Header("Pie Setup")]
//    public GameObject piePrefab;
//    public Sprite[] pieSpritesNormal;
//    public Sprite[] pieSpritesOdd;

//    [Header("Game Settings")]
//    public int totalRounds = 3;
//    public int piesPerRound = 5;
//    public float spacing = 2f;

//    [Header("Feedback")]
//    public Sprite[] feedbackSpritesCorrect;
//    public Sprite[] feedbackSpritesWrong;
//    public Image feedbackImage;
//    public float feedbackDisplayTime = 1.5f;

//    private int currentRound = 0;
//    private GameObject currentOddPie;

//    void Start()
//    {
//        StartNextRound();
//    }

//    void StartNextRound()
//    {
//        ClearPies();

//        currentRound++;

//        if (currentRound > totalRounds)
//        {
//            Debug.Log("🎉 You Win All Rounds!");

//            // ✅ Award 1 point after all rounds completed
//            ScoreManager.Instance.AddScore(1);

//            // Return to Wheel
//            SceneManager.LoadScene("Wheel");
//            return;
//        }

//        int oddIndex = Random.Range(0, piesPerRound);

//        for (int i = 0; i < piesPerRound; i++)
//        {
//            Vector3 pos = new Vector3((i - piesPerRound / 2f) * spacing, 0f, 0f);
//            GameObject pie = Instantiate(piePrefab, pos, Quaternion.identity);
//            pie.tag = "Pie";

//            SpriteRenderer sr = pie.GetComponent<SpriteRenderer>();
//            Pie pieScript = pie.GetComponent<Pie>();

//            if (i == oddIndex)
//            {
//                sr.sprite = pieSpritesOdd[Random.Range(0, pieSpritesOdd.Length)];
//                pieScript.SetOdd(true);
//                currentOddPie = pie;
//            }
//            else
//            {
//                sr.sprite = pieSpritesNormal[Random.Range(0, pieSpritesNormal.Length)];
//                pieScript.SetOdd(false);
//            }
//        }

//        Debug.Log($"▶️ Round {currentRound} started. Find the WRONG pie!");
//    }

//    public void HandlePieTapped(bool wasOdd)
//    {
//        ShowFeedback(wasOdd);
//        StartCoroutine(ContinueAfterDelay(wasOdd));
//    }

//    void ShowFeedback(bool wasCorrect)
//    {
//        Sprite[] selectedArray = wasCorrect ? feedbackSpritesWrong : feedbackSpritesCorrect;

//        if (selectedArray.Length == 0)
//        {
//            Debug.LogWarning("⚠️ No feedback images assigned!");
//            return;
//        }

//        Sprite randomSprite = selectedArray[Random.Range(0, selectedArray.Length)];
//        feedbackImage.sprite = randomSprite;
//        feedbackImage.gameObject.SetActive(true);
//    }

//    IEnumerator ContinueAfterDelay(bool wasCorrect)
//    {
//        yield return new WaitForSeconds(feedbackDisplayTime);
//        feedbackImage.gameObject.SetActive(false);

//        if (wasCorrect)
//        {
//            StartNextRound();
//        }
//        else
//        {
//            Debug.Log("❌ Wrong pie tapped. Game Over.");
//            SceneManager.LoadScene("Wheel");
//        }
//    }

//    void ClearPies()
//    {
//        GameObject[] allPies = GameObject.FindGameObjectsWithTag("Pie");
//        foreach (GameObject pie in allPies)
//        {
//            Destroy(pie);
//        }
//    }
//}
