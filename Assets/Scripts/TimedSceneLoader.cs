using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimedSceneLoader : MonoBehaviour
{
    [Tooltip("Time in seconds before switching to the 'Wheel' scene.")]
    public float timeUntilSwitch = 10f;

    [Tooltip("Reference to the Image that fills the timer bar.")]
    public Image timerBarFill;

    private float timer;

    void Start()
    {
        timer = timeUntilSwitch;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Prevent negative time
        if (timer < 0f)
        {
            timer = 0f;
        }

        // Update fill amount (normalized: 0 = empty, 1 = full)
        if (timerBarFill != null)
        {
            timerBarFill.fillAmount = timer / timeUntilSwitch;
        }

        if (timer <= 0f)
        {
            SceneManager.LoadScene("Wheel");
        }
    }
}
