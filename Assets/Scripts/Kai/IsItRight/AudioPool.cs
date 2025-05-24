using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    [Header("Audio Pool Settings")]
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> audioSources;



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
}
