using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PieShooter : MonoBehaviour
{
    public GameObject piePrefab;
    public Transform firePoint;
    public float pieSpeed = 10f;
    public bool autoShoot = false;
    public float throwInterval = 2f;
    private float timer;

    void Update()
    {
        if (autoShoot)
        {
            timer += Time.deltaTime;
            if (timer >= throwInterval)
            {
                timer = 0f;
                ShootPie();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootPie();
        }
    }

    void ShootPie()
    {
        GameObject pie = Instantiate(piePrefab, firePoint.position, Quaternion.identity);
        pie.GetComponent<Pie>().Launch(pieSpeed);
    }
}




