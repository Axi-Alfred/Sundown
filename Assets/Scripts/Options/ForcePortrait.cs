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

        Time.timeScale = GameManager1.gameSpeedMultiplier; //Viktig rad f�r att se till att game speeden faktiskt �ndras. Om du �ndrar n�t i framtiden se till att den h�r e med och att den e med sist av allt.
    }

    // Optional: keep Start if you want auto-run in some scenes
    void Start()
    {
        Enforce();
    }
}
