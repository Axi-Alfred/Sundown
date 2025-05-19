using UnityEngine;

public class ForcePortrait : MonoBehaviour
{
    // Use this method to trigger portrait orientation manually
    public void Enforce()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Optional: keep Start if you want auto-run in some scenes
    void Start()
    {
        Enforce();
    }
}
