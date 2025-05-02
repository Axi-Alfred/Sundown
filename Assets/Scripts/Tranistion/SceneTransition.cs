using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static Animator animator;
    private static string nextSceneToLoad;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void FadeOut(string scene)
    {
        nextSceneToLoad = scene;
        animator.SetTrigger("FadeOut");
    }

    public void LoadSceneAfterTransition()
    {
        SceneManager.LoadScene(nextSceneToLoad);
    }
}
