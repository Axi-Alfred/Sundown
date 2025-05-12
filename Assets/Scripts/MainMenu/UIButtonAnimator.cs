using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayClickAnimation()
    {
        Debug.Log("Klickade på knappen!");
        animator.SetTrigger("Pressed");
    }
}
