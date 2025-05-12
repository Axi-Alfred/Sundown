using UnityEngine;

public class UndoOffScreen : MonoBehaviour
{
    [Header("Destroy Boundaries")]
    public float rightLimit = 15f;
    public float bottomLimit = -10f;

    [Header("Clown Crash Settings")]
    private static int crashedClowns = 0;
    public static int maxCrashes = 20;

    void Update()
    {
        bool isOffRight = transform.position.x > rightLimit;
        bool isOffBottom = transform.position.y < bottomLimit;

        if (isOffRight || isOffBottom)
        {
            if (gameObject.CompareTag("Clown"))
            {
                crashedClowns++;
                Debug.Log($"💥 Clown crashed! Total: {crashedClowns}");

                if (crashedClowns >= maxCrashes)
                {
                    Debug.Log("🛑 Game over — too many clowns crashed.");
                    Time.timeScale = 0f; // Freeze the game
                    GameManager1.EndTurn(); // Or load GameOver scene
                }
            }

            Destroy(gameObject);
        }
    }
}
