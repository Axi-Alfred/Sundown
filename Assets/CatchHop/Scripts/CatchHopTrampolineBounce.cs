using System.Collections;
using UnityEngine;

public class CatchHopTrampolineBounce : MonoBehaviour
{
    public float upwardForce = 10f;       // Hur högt clownen studsar
    public float maxSideForce = 5f;       // Maximal kraft åt sidan

    public Animator trampolineAnimator;   // Animator för trampolinen

    void Start()
    {
        // Se till att trampolinen startar i idle-läget
        if (trampolineAnimator != null)
        {
            trampolineAnimator.SetBool("isBouncing", false);
        }
    }

    private IEnumerator ResetBounceFlag()
    {
        // Vänta lite tills Bounce startar
        yield return new WaitForSeconds(0.1f);

        // Stäng av flaggan så Idle kan nås
        trampolineAnimator.SetBool("isBouncing", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;

        // Kontrollera om det som träffar trampolinen är en clown
        if (hitObject.CompareTag("Clown"))
        {
            Rigidbody2D clownRb = hitObject.GetComponent<Rigidbody2D>();

            if (clownRb != null)
            {
                // Nollställ fallhastigheten (bara y-led)
                Vector2 currentVelocity = clownRb.velocity;
                currentVelocity.y = 0f;
                clownRb.velocity = currentVelocity;

                // Räkna ut var clownen träffade trampolinen
                Vector3 clownPosition = clownRb.transform.position;
                Vector3 trampolinePosition = transform.position;
                float xDifference = clownPosition.x - trampolinePosition.x;

                // Bestäm hur mycket kraft som ska ges åt sidan
                float sideForce = 0f;
                if (xDifference < 0f)
                {
                    sideForce = -maxSideForce;
                }
                else if (xDifference > 0f)
                {
                    sideForce = maxSideForce;
                }

                // Skapa kraftvektor
                Vector2 bounceForce = new Vector2(sideForce, upwardForce);

                // Applicera kraften
                clownRb.AddForce(bounceForce, ForceMode2D.Impulse);

                // Trigga bounce-animation
                if (trampolineAnimator != null)
                {
                    trampolineAnimator.SetBool("isBouncing", true);
                    StartCoroutine(ResetBounceFlag());
                }

                Debug.Log("BOUNCE triggered at " + Time.time);
            }
        }
    }
}