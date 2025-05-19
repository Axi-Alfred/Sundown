using UnityEngine;
using System.Collections;

public class ForcedLandscape : MonoBehaviour
{
    [Header("Orientation UI")]
    public GameObject orientationWarningUI;

    public IEnumerator StartEnforcing()
    {
        ForceLandscape();

        // 1. Wait 1 second (unscaled)
        yield return new WaitForSecondsRealtime(1f);

        // 2. Freeze the game
        Time.timeScale = 0f;

        // 3. Show message during freeze
        if (orientationWarningUI != null)
            orientationWarningUI.SetActive(true);

        // 4. Wait 2 seconds while frozen
        yield return new WaitForSecondsRealtime(2f);

        // 5. Unfreeze the game
        Time.timeScale = 1f;

        if (orientationWarningUI != null)
            orientationWarningUI.SetActive(false);
    }

    private void ForceLandscape()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }
}
