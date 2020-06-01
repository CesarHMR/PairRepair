using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerDown : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerJump playerJump;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if(playerInput.DownInput() && playerJump.onFoxRange)//if the player input down, find the plataform bellow and call function Down()
        {
            Physics2D.OverlapCircle(playerJump.feetPos.position, playerJump.foxRange, playerJump.groundLayer).GetComponent<Floor>().Down();
        }
    }
}
