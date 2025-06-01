using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour

    // Det här ett script som möjliggör att flera SFX ljud kan spelas uppp samtidigt. Jag hade problem förut med att ljud gick sönder när dom spelades upp samtidigt
{
    [Header("Audio Pool Settings")]
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> audioSources;

    private void Awake()
    {
        audioSources = new List<AudioSource>();

        // Skapa en lista på antalet audioSource instanser. 
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.playOnAwake = false;
            source.loop = false;
            audioSources.Add(source);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        foreach (AudioSource source in audioSources) // Leta eftter tillgängliga audioSources
        {
            if (!source.isPlaying) // Spela endast upp ett klipp om audioSourcen inte är upptagen med ett annat ljudklipp
            {
                source.clip = clip;
                source.volume = volume;
                source.pitch = pitch;
                source.loop = false;
                source.Play();
                return;
            }
        }
        // Om audioSourcen är upptagen, spela ett nytt klipp
        audioSources[0].clip = clip;
        audioSources[0].volume = volume;
        audioSources[0].pitch = pitch;
        audioSources[0].loop = false;
        audioSources[0].Play();
    }
}
