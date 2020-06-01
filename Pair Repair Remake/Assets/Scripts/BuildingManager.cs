using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    #region Variables
    public static int gameMode; //0 to coop ------- 1 to vs
    public static float gameTime;//track to show on the win timer

    [Header("Difficulty Configurations")]
    [SerializeField] private float timeToBreak;
    [SerializeField] private int howManyTurns;
    [SerializeField] private int howManyWalls;
    [SerializeField] private int maxWallsPerTurnSingle;
    [SerializeField] private int maxWallsPerTurnMulti;

    /////////////////////track variables
    int turns;
    int walls;
    int turnModifier;
    float timeModifier;
    int maxWalls;
    ////////////////////
    int rand;
    float t;//loop time to break;
    public static bool gameRunning;

    public List<WallMaterial> wallsPlayerOne = new List<WallMaterial>();
    public List<WallMaterial> wallsPlayerTwo = new List<WallMaterial>();

    public List<WallMaterial> wallsDestroyedPlayerOne = new List<WallMaterial>();
    public List<WallMaterial> wallsDestroyedPlayerTwo = new List<WallMaterial>();

    public List<WallMaterial> wallsDestroyedSinglePlayer = new List<WallMaterial>();

    AudioManager am;
    CanvasLife cl;
    Paralax pl;
    #endregion

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        cl = FindObjectOfType<CanvasLife>();
        pl = FindObjectOfType<Paralax>();

        foreach (WallMaterial w in FindObjectsOfType<WallMaterial>())
        {
            if (w.transform.position.x < 0)//left walls belong to player 1 and right walls to player 2
            {
                wallsPlayerOne.Add(w);
            }
            else
            {
                wallsPlayerTwo.Add(w);
            }
        }
        ///////////////////////////////
        GameEvents.OnGameStart.AddListener(SetNewGame);
        GameEvents.OnGameEnd.AddListener(StopGame);
    }

    private void Update()//Main loop
    {
        if (t + timeModifier <= 0 && gameRunning)
        {
            BreakAnyWall();
            t = timeToBreak;
        }
        else
        {
            t -= Time.deltaTime;
        }

        if (gameRunning)
            gameTime += Time.deltaTime;
    }

    void BreakAnyWall()
    {
        am.Play("Broken");

        for (int i = 0; i < walls; i++)//Choose a number of walls of each side and break it
        {
            rand = Random.Range(0, wallsPlayerOne.Count);
            wallsPlayerOne[rand].ChangeWallState(WallMaterial.wallState.broken);
            wallsDestroyedPlayerOne.Add(wallsPlayerOne[rand]);
            wallsDestroyedSinglePlayer.Add(wallsPlayerOne[rand]);
            wallsPlayerOne.Remove(wallsPlayerOne[rand]);
            ////////////////////////////////////////////
            rand = Random.Range(0, wallsPlayerTwo.Count);
            wallsPlayerTwo[rand].ChangeWallState(WallMaterial.wallState.broken);
            wallsDestroyedPlayerTwo.Add(wallsPlayerTwo[rand]);
            wallsDestroyedSinglePlayer.Add(wallsPlayerTwo[rand]);
            wallsPlayerTwo.Remove(wallsPlayerTwo[rand]);

            //UI life
            cl.ChangeLifeBlue(-1);
            cl.ChangeLifeRed(-1);

            if (CheckWinCondition())
                return;
        }

        turns--;
        CheckTurn();
    }

    public bool CheckWinCondition()
    {
        if(wallsPlayerOne.Count == 0 || wallsPlayerTwo.Count == 0)//if there is no more walls to break, finish the game. Note: no tie feature
        {
            int playerId = wallsPlayerOne.Count == 0 ? 2 : 1;//Finde the winner

            if (gameMode == 0)//COOP or SOLO mode just have the timer and dont have a winner
            {
                pl.SetTimer(gameTime);
            }
            else//VS mode shows just the winner, but not the timer
            {
                pl.SetVsWinner(playerId);
            }

            GameEvents.OnGameEnd.Invoke();
            return true;
        }
        return false;
    }

    void CheckTurn()
    {
        //turnModifier and timeModifier creat a difficulty curve, while maxWalls prevents to become insanely hard to the point of being impossible

        if (turns + turnModifier <= 0)
        {
            turns = howManyTurns;
            if(walls < maxWalls)
            {
                turnModifier++;
                timeModifier += 0.5f;
                walls += turns + turnModifier >= 6 ? 2 : 1; 
            }
        }
    }

    void SetNewGame()
    {
        maxWalls = LevelManager.singleplayer ? maxWallsPerTurnSingle : maxWallsPerTurnMulti;
        turns = howManyTurns;
        walls = howManyWalls;
        turnModifier = 0;
        timeModifier = 0;
        t = 2.6f;//amount of time to synchronize with the music
        gameRunning = true;
        gameTime = 0;
    }

    public void GoldenFix(int playerId, bool fixBrick)//fix all broken walls of a certain material of the player who picks the golden Tool
    {
        am.Play("Golden");

        WallMaterial[] wallsToClear;//Array to set what walls should fix according the material, game mode and player

        if (LevelManager.singleplayer)
        {
            wallsToClear = wallsDestroyedSinglePlayer.ToArray();
        }
        else
        {
            wallsToClear = playerId == 0 ? wallsDestroyedPlayerOne.ToArray() : wallsDestroyedPlayerTwo.ToArray();
        }


        foreach (WallMaterial w in wallsToClear)
        {
            if (w.thisState == WallMaterial.wallState.broken || w.thisState == WallMaterial.wallState.repaired)//if the walls is broken or half repaired
            {
                if ((w.thisMaterial == WallMaterial.material.Brick && fixBrick == true) || (w.thisMaterial == WallMaterial.material.Wood && fixBrick == false))//hammer => wood , trowel => brick
                {
                    w.ChangeWallState(WallMaterial.wallState.clean);
                }
            }
        }
    }

    void StopGame()
    {
        gameRunning = false;
    }
}
