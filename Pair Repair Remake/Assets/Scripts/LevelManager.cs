using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private GameObject playerOneRespawn;
    [SerializeField] private GameObject playerTwo;

    public static bool singleplayer;

    public void SetSingleMode()
    {
        singleplayer = true;
        invisibleWall.SetActive(false);
        playerTwo.SetActive(false);
        playerOneRespawn.transform.position = new Vector2(0, -6.5f);
    }

    public void SetMultiMode()
    {
        singleplayer = false;
        invisibleWall.SetActive(true);
        playerTwo.SetActive(true);
        playerOneRespawn.transform.position = new Vector2(-5.5f, -6.5f);
    }
}
