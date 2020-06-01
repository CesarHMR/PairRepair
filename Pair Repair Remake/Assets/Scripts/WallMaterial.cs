using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaterial : MonoBehaviour
{
    public enum material { Wood, Brick };
    public enum wallState { broken, repaired, clean}

    public material thisMaterial;
    public wallState thisState;

    WallSprite wallSprite;
    BuildingManager bm;
    AudioManager am;

    private void Awake()
    {
        wallSprite = GetComponent<WallSprite>();
        bm = FindObjectOfType<BuildingManager>();
        am = FindObjectOfType<AudioManager>();
        ////////////////////////////////////
        GameEvents.OnGameStart.AddListener(StartGame);
    }

    public void ChangeWallState(wallState stateToChange)
    {
        switch (stateToChange)
        {
            case wallState.broken:
                BreakSound();
                break;
            case wallState.repaired:
                RepairSound();

                break;
            case wallState.clean:
                CleanSound();
                break;
        }
        thisState = stateToChange;
        wallSprite.TradeSprite(stateToChange, thisMaterial);
    }

    public void Interact()
    {
        if(thisState == wallState.broken)
        {
            ChangeWallState(wallState.repaired);
        }
        else if(thisState == wallState.repaired)
        {
            ChangeWallState(wallState.clean);
        }
    }

    void BreakSound()
    {
        am.Play("Broken");
    }

    void RepairSound()
    {
        string soundName = thisMaterial == material.Wood ? "Plank" : "Cemment";
        am.Play(soundName);
    }

    public void CleanSound()
    {
        if (transform.position.x > 0)
        {
            bm.wallsDestroyedPlayerTwo.Remove(this);
            if (!bm.wallsPlayerOne.Contains(this))
            {
                bm.wallsPlayerTwo.Add(this);
            }
            FindObjectOfType<CanvasLife>().ChangeLifeRed(1);
            am.Play("Repair_2");
        }
        else
        {
            bm.wallsDestroyedPlayerOne.Remove(this);
            if (!bm.wallsPlayerOne.Contains(this))
            {
                bm.wallsPlayerOne.Add(this);
            }
            FindObjectOfType<CanvasLife>().ChangeLifeBlue(1);
            am.Play("Repair_1");
        }

        bm.wallsDestroyedSinglePlayer.Remove(this);
    }
    public void StartGame()
    {
        if (transform.position.x > 0)
        {
            if (bm.wallsDestroyedPlayerTwo.Contains(this))
            {
                bm.wallsDestroyedPlayerTwo.Remove(this);
            }

            if (!bm.wallsPlayerTwo.Contains(this))
            {
                bm.wallsPlayerTwo.Add(this);
            }
        }
        else
        {
            if (bm.wallsDestroyedPlayerOne.Contains(this))
            {
                bm.wallsDestroyedPlayerOne.Remove(this);
            }

            if (!bm.wallsPlayerOne.Contains(this))
            {
                bm.wallsPlayerOne.Add(this);
            }
        }

        bm.wallsDestroyedSinglePlayer.Remove(this);
    }
}
