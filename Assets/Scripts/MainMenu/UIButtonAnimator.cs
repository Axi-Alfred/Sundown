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

    public void PlayClickAndThen(System.Action afterDelay)
    {
        StartCoroutine(PlayThenRun(afterDelay));
    }

    private IEnumerator PlayThenRun(System.Action callback)
    {
        if (animator != null)
        {
            animator.SetTrigger("Pressed");
        }

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSeconds(0.2f); // Anpassa till längd på ljud/animation

        callback?.Invoke();
    }

}
