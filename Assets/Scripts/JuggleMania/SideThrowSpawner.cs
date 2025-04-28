using UnityEngine;

public class SideThrowSpawner : MonoBehaviour
{
    public GameObject pinPrefab;
    public GameObject knifePrefab;

    public float spawnInterval = 2.5f;
    public float sideOffset = 2f; // how far offscreen to spawn
    public float arcHeight = 7f;
    public float throwDuration = 1.0f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnThrow), 1f, spawnInterval);
    }

    void SpawnThrow()
    {
        var availableSpots = SpotManager.Instance.GetAvailableSpots(null);
        if (availableSpots.Count == 0)
            return;

        SpotController targetSpot = availableSpots[Random.Range(0, availableSpots.Count)];

        // Decide left or right side spawn
        bool spawnLeft = Random.value > 0.5f;

        // Get Y position roughly aligned with target spot, but slightly randomized
        Vector3 targetPosition = targetSpot.transform.position;
        Vector3 spawnPosition = targetPosition;

        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        if (spawnLeft)
            spawnPosition.x = mainCamera.transform.position.x - camWidth / 2f - sideOffset;
        else
            spawnPosition.x = mainCamera.transform.position.x + camWidth / 2f + sideOffset;

        // Optionally randomize Y a little
        spawnPosition.y += Random.Range(-0.5f, 0.5f);

        // Spawn object
        float roll = Random.value;
        GameObject prefab = (roll < 0.8f) ? pinPrefab : knifePrefab;
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        var jugglingObj = obj.GetComponent<JugglingObject>();
        jugglingObj.isKnife = (prefab == knifePrefab);
        jugglingObj.currentSpot = targetSpot;
        targetSpot.currentObject = jugglingObj;

        // Launch into the target
        jugglingObj.LaunchToSpot(targetPosition, arcHeight, throwDuration);
    }
}
