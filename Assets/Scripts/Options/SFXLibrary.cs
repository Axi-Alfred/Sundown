using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXLibrary : MonoBehaviour
{
    public List<NumberedAudioClip> sounds = new List<NumberedAudioClip>();

    private AudioSource audioSource;

    void Awake()
    {
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

        var clip = entry.clips[Random.Range(0, entry.clips.Count)]; // 🔀 pick one randomly

        // Random pitch/volume (per group)
        audioSource.pitch = Random.Range(entry.pitchMin, entry.pitchMax);
        audioSource.volume = Random.Range(entry.volumeMin, entry.volumeMax);

        audioSource.PlayOneShot(clip);

        // Reset after
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
    }


}
