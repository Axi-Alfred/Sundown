using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [Tooltip("Set the duration for the minigame timer in seconds.")]
    public float minigameDuration = 10f;

    public UnityEvent OnTimerEnd;

    private Coroutine timerCoroutine;
    public float CurrentTime { get; private set; }

    void Start()
    {
        StartTimer(minigameDuration);
    }

    public void StartTimer(float duration)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(RunTimer(duration));
    }

    IEnumerator RunTimer(float duration)
    {
        CurrentTime = duration;

        while (CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            yield return null;
        }

        CurrentTime = 0;
        OnTimerEnd?.Invoke();
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }
}