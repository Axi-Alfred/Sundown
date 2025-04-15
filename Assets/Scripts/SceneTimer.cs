using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    [Tooltip("Time in seconds before switching to the 'Wheel' scene.")]
    public float timeUntilSwitch = 5f;

    private float timer;

    void Start()
    {
        timer = timeUntilSwitch;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SceneManager.LoadScene("Wheel");
        }
    }
}
