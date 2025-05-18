using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public AudioClip musicClip;
    public float delayInSeconds = 0f; // 🕒 How long to wait before starting music

    private AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = musicClip;
        audio.loop = true;
        audio.playOnAwake = false;

        // Optional: Keep playing across scenes
        DontDestroyOnLoad(gameObject);

        // Delay playback
        if (delayInSeconds > 0)
            Invoke(nameof(PlayMusic), delayInSeconds);
        else
            PlayMusic();
    }

    private void PlayMusic()
    {
        if (musicClip != null)
        {
            audio.Play();
        }
    }
}
