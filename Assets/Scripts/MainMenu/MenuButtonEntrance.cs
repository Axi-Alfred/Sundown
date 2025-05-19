using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonEntrance : MonoBehaviour
{
    public Animator[] buttonAnimators;

    void Start()
    {
        foreach (Animator anim in buttonAnimators)
        {
            anim.Play("FlyIn");
        }
    }
}
