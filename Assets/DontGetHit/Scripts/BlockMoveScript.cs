using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveScript : MonoBehaviour
{
    private float deadZone = -11.88f;
    private bool isDeadly = true; // 🧨 Används för att sluta skada spelaren

    void Update()
    {
        float currentSpeed = DifficultyManagerScript.Instance.GetCurrentSpeed();
        transform.position += Vector3.down * currentSpeed * Time.deltaTime;

        // Om blocket hamnar långt nedanför skärmen, ta bort det
        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
            Debug.Log("DESTROYED: Out of bounds");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isDeadly = false; // 🔪 Blocket är inte längre farligt
            Destroy(gameObject); // 💥 Ta bort blocket så att spelaren kan röra sig fritt
            Debug.Log("Block hit ground and was destroyed");
        }
    }

    // 👉 Den här metoden kan kallas av PlayerScript (valfritt)
    public bool IsDeadly()
    {
        return isDeadly;
    }
}
