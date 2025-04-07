using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{

    public float movieSpeed = 5;
    private double deadZone = -11.88;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = transform.position + (Vector3.down * movieSpeed) * Time.deltaTime;

       if(transform.position.y < deadZone){
        //Debug.Log("Block destroyed");
        Destroy(gameObject);
       }
    }

   
}
