using UnityEngine;

public class StarBurstController : MonoBehaviour
{
    public ParticleSystem top, bottom, left, right;
    public AudioSource chime;

    public void TriggerBurst()
    {
        Debug.Log("Star Burst Triggered!");
        if (top != null) top.Play();
        if (bottom != null) bottom.Play();
        if (left != null) left.Play();
        if (right != null) right.Play();
        if (chime != null) chime.Play();
    }
}
