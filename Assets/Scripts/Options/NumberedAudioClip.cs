using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NumberedAudioClip
{
    public List<AudioClip> clips = new List<AudioClip>(); // 👈 support multiple clips
    [TextArea(1, 3)]
    public string description;

    // Optional: pitch/volume per group
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;
    public float volumeMin = 0.9f;
    public float volumeMax = 1f;
}
