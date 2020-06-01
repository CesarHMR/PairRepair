using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeToRespawn;
    [SerializeField] private float respawnJumpForce;
    Transform respawnPoint;

    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        //////////////////////////
        GameEvents.OnGameStart.AddListener(AssingRespawnPoint);
        GameEvents.OnGameStart.AddListener(StartGame);
        GameEvents.OnGameEnd.AddListener(EndGame);
    }

    void AssingRespawnPoint()
    {
        string respawnName = playerInput.id == 0 ? "SpawnP1" : "SpawnP2";
        respawnPoint = GameObject.FindGameObjectWithTag(respawnName).transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DeathBox")
        {
            StartCoroutine(Respawn());
        }
    }

    void StartGame()
    {
        playerInput.am.Play("Up");
        transform.position = respawnPoint.position;
        playerInput.rb.velocity = new Vector2(0, respawnJumpForce);
    }

    void EndGame()
    {
        if(gameObject.active == true)
            StartCoroutine(Despawn());
        else
        {
            gameObject.SetActive(true);
            gameObject.transform.position = new Vector2(0, -11);
        }
    }

    IEnumerator Respawn()//When the player fall, wait a moment, put in the respawn position and make it jump
    {
        playerInput.am.Play("Die");
        //--------------------------//
        yield return new WaitForSeconds(timeToRespawn);
        //--------------------------//
        playerInput.am.Play("Up");
        transform.position = respawnPoint.position;
        playerInput.rb.velocity = new Vector2(0, respawnJumpForce);
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2f);
        transform.position = new Vector2(0, -10);
    }
}
