using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerJumpFix : MonoBehaviour
{
    [SerializeField] private float gravityDefaut, gravityFall;

    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!playerInput.canMove)
            return;


        if(playerInput.rb.velocity.y < 0)//gravity on fall is stronger than default gravity, make the jump less float
        {
            playerInput.rb.gravityScale = gravityFall;
        }
        else
        {
            playerInput.rb.gravityScale = gravityDefaut;
        }
    }
}
