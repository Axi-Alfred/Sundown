using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlatform;
    [SerializeField] private GameObject spawner;

    private void Start()
    {
        defaultPlatform.SetActive(false);
        spawner.SetActive(false);
    }
    public void AfterCountdown()
    {
        defaultPlatform.SetActive(true);
        spawner.SetActive(true);
        gameObject.SetActive(false);
    }
}
