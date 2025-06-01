using UnityEngine;

public class CatchHopOffScreenKillZone : MonoBehaviour
{
    private float leftLimit;
    private float rightLimit;
    private float topLimit;
    private float bottomLimit;

    private static int crashedClownCount = 0;
    private static int maxCrashes = 20;

    private float zDistanceToCamera;

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Ingen MainCamera hittades! Tagga din kamera som 'MainCamera'.");
            return;
        }

        // Använd z-positionen 0 som vi antar att dina projektliler ligger på
        zDistanceToCamera = Mathf.Abs(Camera.main.transform.position.z);

        CalculateKillZone();
    }

    void Update()
    {
        // Leta upp alla objekt med taggen "Projectile"
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Clown");

        foreach (GameObject obj in projectiles)
        {
            Vector3 pos = obj.transform.position;

            if (pos.x < leftLimit || pos.x > rightLimit || pos.y > topLimit || pos.y < bottomLimit)
            {
                // Om det är en clown (lägg till taggen "Clown" på dem)
                if (obj.CompareTag("Clown"))
                {
                    crashedClownCount++;

                    if (crashedClownCount >= maxCrashes)
                    {
                        Debug.Log("Spelet är slut – 20 clowner har kraschat.");
                        Time.timeScale = 0f;
                    }
                }

                Destroy(obj);
            }
        }
    }

    void CalculateKillZone()
    {
        Vector3 screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, zDistanceToCamera));
        Vector3 screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, zDistanceToCamera));
        Vector3 screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, zDistanceToCamera));
        Vector3 screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, zDistanceToCamera));

        float margin = 1f;

        leftLimit = screenLeft.x - margin;
        rightLimit = screenRight.x + margin;
        topLimit = screenTop.y + margin;
        bottomLimit = screenBottom.y - margin;

        Debug.Log("KillZone boundaries:");
        Debug.Log($"Left: {leftLimit}, Right: {rightLimit}, Top: {topLimit}, Bottom: {bottomLimit}");
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || Camera.main == null)
            return;

        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(leftLimit, topLimit, 0);
        Vector3 topRight = new Vector3(rightLimit, topLimit, 0);
        Vector3 bottomRight = new Vector3(rightLimit, bottomLimit, 0);
        Vector3 bottomLeft = new Vector3(leftLimit, bottomLimit, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}