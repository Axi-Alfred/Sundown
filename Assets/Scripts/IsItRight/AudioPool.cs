using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> audioSources;

    private void Awake()
    {
        audioSources = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.playOnAwake = false;
            audioSources.Add(source);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.volume = volume;
                source.Play();
                return;
            }
        }

        // If all sources are busy, just play on first (fallback)
        audioSources[0].clip = clip;
        audioSources[0].volume = volume;
        audioSources[0].Play();
    }
}

