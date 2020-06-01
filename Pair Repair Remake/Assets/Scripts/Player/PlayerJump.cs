using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    public Transform feetPos;//PlayerDown.cs
    public float checkRange, foxRange;//PlayerDown.cs
    public LayerMask groundLayer;//PlayerDown.cs

    public bool isGrounded;
    public bool onFoxRange;
    bool jumpSet;

    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRange, groundLayer);
        onFoxRange = Physics2D.OverlapCircle(feetPos.position, checkRange, groundLayer);

        if (playerInput.JumpInput() && !CanvasLife.paused)
        {
            if (isGrounded || onFoxRange)
            {
                jumpSet = true;
            }
        }

        if (jumpSet && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        jumpSet = false;
        playerInput.rb.velocity = new Vector2(playerInput.rb.velocity.x, jumpForce);
        playerInput.am.Play("Jump" + playerInput.id);
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetPos.position, checkRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feetPos.position, foxRange);
    }*/
}
