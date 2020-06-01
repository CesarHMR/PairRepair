using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(Animator))]

public class Floor : MonoBehaviour
{
    bool lowered;//if the plataform already is down

    PlatformEffector2D effector;//efecctor that makes collision only if comes from above
    Animator anim;
    AudioManager am;

    private void Start()
    {
        lowered = false;

        effector = GetComponent<PlatformEffector2D>();
        anim = GetComponent<Animator>();
        am = FindObjectOfType<AudioManager>();
    }

    public void Down()
    {
        if (!lowered)
        {
            StartCoroutine(InvertEffector());
        }
    }

    IEnumerator InvertEffector()//annul the angle of platform effector making the player falls, wait a little and then goes back to normal
    {
        lowered = true;

        am.Play("Fall");
        anim.SetTrigger("Down");
        effector.surfaceArc = 0;
        //-----------------------//
        yield return new WaitForSeconds(0.2f);
        //-----------------------//
        effector.surfaceArc = 180;

        lowered = false;
    }

}
