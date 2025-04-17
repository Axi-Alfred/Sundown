using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public GameObject bulletPrefab;    // Prefab för kulan som ska skjutas
    public Transform shootPoint;       // Punkt där kulan skapas
    public float shootCooldown = 0.2f; // Tid mellan varje skott

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Om spelaren trycker på skärmen och det har gått tillräckligt med tid
        if (Input.touchCount > 0 && timer >= shootCooldown)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Shoot();      // Skjut en kula
                timer = 0f;   // Starta om timern
            }
        }
    }

    void Shoot()
    {
        // Skapa kulan vid skjutpunkten
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
    }
}
