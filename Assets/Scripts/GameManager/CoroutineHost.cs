using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHost : MonoBehaviour
{
    public static CoroutineHost instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
