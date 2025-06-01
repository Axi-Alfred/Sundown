using UnityEngine;

public class TrampolineUniteAnimator : MonoBehaviour
{
    public Animator clownsAnimator; // Animator for ClownHolders

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // Kontrollera om TrampolineUnite rör på sig
        bool isMoving = transform.position != previousPosition;

        // Uppdatera clownens animation
        clownsAnimator.SetBool("isRunning", isMoving);

        // Spara nuvarande position för nästa jämförelse
        previousPosition = transform.position;
    }
}