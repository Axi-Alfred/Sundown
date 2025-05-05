using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject Confetti;

    private void Start()
    {
        Confetti = GetComponent<GameObject>();
    }
}
