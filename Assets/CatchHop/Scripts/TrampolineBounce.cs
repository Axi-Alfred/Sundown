using System.Collections;
using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
    // Dessa värden styr hur mycket clownen studsar uppåt och åt sidan
    public float upwardForce = 10f;       // Hur högt clownen studsar
    public float maxSideForce = 5f;       // Maximal kraft åt sidan

    public Animator trampolineAnimator; // Animator för trampolinen

    private IEnumerator ResetBounceFlag()
    {
        // Vänta lite tills Bounce startar
        yield return new WaitForSeconds(0.1f);

        // Stäng av flaggan så Idle kan nås
        trampolineAnimator.SetBool("isBouncing", false);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kontrollera om det som träffar trampolinen är en clown
        GameObject hitObject = collision.gameObject;

        if (hitObject.CompareTag("Clown"))
        {
            // Hämta clownens Rigidbody2D
            Rigidbody2D clownRb = hitObject.GetComponent<Rigidbody2D>();

            // Kontrollera att Rigidbody2D finns
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

                // Om clownen är till vänster om mitten
                if (xDifference < 0f)
                {
                    sideForce = -maxSideForce;
                }
                // Om clownen är till höger om mitten
                else if (xDifference > 0f)
                {
                    sideForce = maxSideForce;
                }
                // Om clownen är exakt i mitten
                else
                {
                    sideForce = 0f;
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

                    Debug.Log("BOUNCE triggered at " + Time.time);

                }


            }
        }
    }
}