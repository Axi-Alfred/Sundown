using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public AudioClip musicClip;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Optional: survives scene change
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = musicClip;
        audio.loop = true;
        audio.playOnAwake = false;
        audio.Play();
    }
}
