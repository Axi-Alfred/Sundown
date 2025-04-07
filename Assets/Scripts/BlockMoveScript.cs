using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{

    //public float movieSpeed = 5;

    private double deadZone = -11.88;

    void Start()
    {

    }


    void Update()
    {
        /*transform.position = transform.position + (Vector3.down * movieSpeed) * Time.deltaTime;

        if(transform.position.y < deadZone){
         //Debug.Log("Block destroyed");
         Destroy(gameObject);
        }*/

        float currentSpeed = DifficultyManagerScript.Instance.GetCurrentSpeed();
        transform.position += Vector3.down * currentSpeed * Time.deltaTime;

        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
            Debug.Log("DESTROYED");
            {
                
            }
        }
    }


}
