using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PieShooter : MonoBehaviour
{
    public GameObject piePrefab;
    public Transform firePoint;
    public float pieSpeed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject pie = Instantiate(piePrefab, firePoint.position, Quaternion.identity);
            pie.GetComponent<Pie>().Launch(pieSpeed);
        }
    }
}



