using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    public static bool paused;

    PlayerInput playerInput;
    SpriteRenderer sr;
    Animator anim;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!playerInput.canMove)
            return;

        anim.SetBool("isRunning", (playerInput.rb.velocity.x != 0));

        if(playerInput.rb.velocity.x != 0)
        {
            sr.flipX = (playerInput.rb.velocity.x < 0);
        }

        if(playerInput.rb.velocity.y > 0)
        {
            anim.SetTrigger("isJumping");
        }
        else
        {
            anim.SetTrigger("isLanding");
        }
    }
}
