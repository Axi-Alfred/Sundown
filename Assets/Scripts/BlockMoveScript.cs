using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{
    //public float movieSpeed = 5;

    private float deadZone = -11.88f;

    void Start()
    {
        // optional setup here
    }

    void Update()
    {
        float currentSpeed = DifficultyManagerScript.Instance.GetCurrentSpeed();
        transform.position += Vector3.down * currentSpeed * Time.deltaTime;

        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
            Debug.Log("DESTROYED");
        }
    }
}
