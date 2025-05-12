using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAnimator : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private AudioClip clickSound; // Tilldelas i Inspector

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Pressed");
        }

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
