using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private bool wheelHasSpinned;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = wheelHasSpinned;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Game1":
                print("Game1 will begin now");
                break;

            case "Game2":
                print("Game2 will begin now");
                break;

            case "Game3":
                print("Game3 will begin now");
                break;
        }
    }

    public void WheelHasSpinned(bool spinning)
    {
        wheelHasSpinned = spinning;
    }
}
