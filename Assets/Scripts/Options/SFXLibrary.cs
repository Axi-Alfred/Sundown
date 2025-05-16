using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXLibrary : MonoBehaviour
{
    public static SFXLibrary Instance { get; private set; }

    public List<NumberedAudioClip> sounds = new List<NumberedAudioClip>();

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(int index)
    {
        if (index <= 0 || index > sounds.Count)
        {
            Debug.LogWarning($"Invalid SFX index: {index}");
            return;
        }

        var entry = sounds[index - 1];
        if (entry.clips == null || entry.clips.Count == 0) return;

        var clip = entry.clips[Random.Range(0, entry.clips.Count)];

        // Apply random pitch and volume
        audioSource.pitch = Random.Range(entry.pitchMin, entry.pitchMax);
        audioSource.volume = Random.Range(entry.volumeMin, entry.volumeMax);
        audioSource.PlayOneShot(clip);

        // Reset after playing
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
    }
}
