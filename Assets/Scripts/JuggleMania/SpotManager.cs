using System.Collections.Generic;
using UnityEngine;

public class SpotManager : MonoBehaviour
{
    public static SpotManager Instance;
    public int deadHands = 0; // NEW: Tracks how many hands have been lost


    public List<SpotController> allSpots = new List<SpotController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // NO NEED TO REMOVE SPOTS ANYMORE
    public void SpotDied(SpotController spot)
    {
        // Optionally notify other systems
    }

    public int CountDeadSpots()
    {
        int count = 0;
        foreach (var spot in allSpots)
        {
            if (spot != null && !spot.isAlive)
                count++;
        }
        return count;
    }

    public List<SpotController> GetAvailableSpots(SpotController exclude)
    {
        List<SpotController> available = new List<SpotController>();
        foreach (var spot in allSpots)
        {
            if (spot != null && spot.isAlive && spot != exclude)
            {
                available.Add(spot);
            }
        }
        return available;
    }
}
