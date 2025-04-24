using UnityEngine;
using System.Collections;

public class ForcedLandscape : MonoBehaviour
{
    [Header("Orientation UI")]
    public GameObject orientationWarningUI;
                 
    void Start()
    {
        ForceLandscape();
        StartCoroutine(ShowInitialWarning());
    }

    void ForceLandscape()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }

    IEnumerator ShowInitialWarning()
    {
        Time.timeScale = 0f;
        orientationWarningUI.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        orientationWarningUI.SetActive(false);
        Time.timeScale = 1f;

    }
}
