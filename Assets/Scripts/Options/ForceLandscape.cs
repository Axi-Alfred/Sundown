using UnityEngine;

public class ForceLandscapeWithNotice : MonoBehaviour
{
    public GameObject instructionImage;

    void Start()
    {
        // Force orientation to LandscapeLeft only
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;

        // Show instruction image
        if (instructionImage != null)
        {
            instructionImage.SetActive(true);
            Invoke(nameof(HideImage), 3f);
        }
    }

    void HideImage()
    {
        instructionImage.SetActive(false);
    }
}
