using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }
    private float fadeDuration = 0.75f;

    public AudioClip musicClip;
    public float delayInSeconds = 0f; // 🕒 How long to wait before starting music

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;



        // Delay playback
        if (delayInSeconds > 0)
            Invoke(nameof(PlayMusic), delayInSeconds);
        else
            PlayMusic();

        float pitch = audioSource.pitch;
        pitch = pitch * GameManager1.gameSpeedMultiplier;
    }

    private void PlayMusic()
    {
        if (musicClip != null)
        {
            audioSource.Play();
        }
    }

    public void FadeOutMusic()
    {
        audioSource.DOFade(0f, fadeDuration);
    }
}
