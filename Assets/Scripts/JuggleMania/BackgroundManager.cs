using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Sprite initialBackground;
    public Sprite oneDeadBackground;
    public Sprite twoDeadBackground;
    public Sprite threeDeadBackground;

    private Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void UpdateBackground(int deadHands)
    {
        if (deadHands == 0)
        {
            backgroundImage.sprite = initialBackground;
        }
        else if (deadHands == 1)
        {
            backgroundImage.sprite = oneDeadBackground;
        }
        else if (deadHands == 2)
        {
            backgroundImage.sprite = twoDeadBackground;
        }
        else if (deadHands >= 3)
        {
            backgroundImage.sprite = threeDeadBackground;
        }
    }
}
