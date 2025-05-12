using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    [Header("Audio Pool Settings")]
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> audioSources;

    // ✅ Dedicated Background Music Source
    private AudioSource backgroundSource;

    private void Awake()
    {
        audioSources = new List<AudioSource>();

        // Create Pooled SFX Sources
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.playOnAwake = false;
            source.loop = false; // SFX should never loop
            audioSources.Add(source);
        }

        // Create Dedicated Background Music Source
        backgroundSource = Instantiate(audioSourcePrefab, transform);
        backgroundSource.playOnAwake = false;
        backgroundSource.loop = true; // ✅ Only this source loops
    }

    // ✅ Play Sound Effect (SFX)
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.volume = volume;
                source.pitch = pitch;
                source.loop = false; // ✅ SFX never loops
                source.Play();
                return;
            }
        }

        // If all sources are busy, use the first one
        audioSources[0].clip = clip;
        audioSources[0].volume = volume;
        audioSources[0].pitch = pitch;
        audioSources[0].loop = false;
        audioSources[0].Play();
    }

    // ✅ Play Looping Background Music
    public void PlayBackgroundMusic(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (backgroundSource == null) return;

        if (backgroundSource.isPlaying && backgroundSource.clip == clip)
            return; // Prevent restarting the same music

        backgroundSource.clip = clip;
        backgroundSource.volume = volume;
        backgroundSource.pitch = pitch;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    // ✅ Stop Background Music
    public void StopBackgroundMusic()
    {
        if (backgroundSource != null)
            backgroundSource.Stop();
    }

    // ✅ Fade Background Music In/Out
    public void FadeBackgroundMusic(float targetVolume, float duration)
    {
        if (backgroundSource == null) return;
        StartCoroutine(FadeMusicCoroutine(targetVolume, duration));
    }

    private System.Collections.IEnumerator FadeMusicCoroutine(float targetVolume, float duration)
    {
        float startVolume = backgroundSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            backgroundSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }

        backgroundSource.volume = targetVolume;
        if (targetVolume == 0f) backgroundSource.Stop();
    }
}
