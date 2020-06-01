using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paralax : MonoBehaviour
{
    Animator cameraAnim;
    public Animator interfaceAnim;
    bool canClick;//Button to avod trigger buttons twice
    public GameObject interfaceComponent;
    public GameObject Solo;
    public GameObject Coop;
    public GameObject Vs;
    public GameObject TimerUI;
    public GameObject WinnerImage;
    public Sprite[] playersSprites;
    public AudioMixer audioMixer;

    AudioManager am;

    private void Awake()
    {
        am = FindObjectOfType<AudioManager>();
        canClick = true;
        cameraAnim = GetComponent<Animator>();
        GameEvents.OnGameEnd.AddListener(Menu);
    }

    public void StartGame()//When scene is seted to gameplay, the game start
    {
        GameEvents.OnGameStart.Invoke();
        canClick = true;
        Coop.SetActive(false);
        Vs.SetActive(false);
        Solo.SetActive(false);
        //interfaceComponent.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()//Trigger to go back to menu
    {
        //interfaceComponent.SetActive(true);
        cameraAnim.SetTrigger("Menu");
        interfaceAnim.SetTrigger("Menu");
    }

    public void ShowGameModes()
    {
        Coop.SetActive(true);
        Vs.SetActive(true);
        Solo.SetActive(true);
    }

    public void SetCoop()
    {
        if (canClick)
        {
            BuildingManager.gameMode = 0;
            SetCameraPlay();
        }
    }

    public void SetVs()
    {
        if (canClick)
        {
            BuildingManager.gameMode = 1;
            SetCameraPlay();
        }
    }

    void SetCameraPlay()
    {
        EventSystem.current.SetSelectedGameObject(null);
        canClick = false;
        cameraAnim.SetTrigger("Play");
        interfaceAnim.SetTrigger("Play");
        am.FadeInOut("Morning", 0.2f, true, 1);
        am.Play("Street");
        am.FadeInOut("Street", 0.2f, false, 0.05f);
        am.Play("Whoosh");
    }

    public void SetTimer(float time)
    {
        WinnerImage.SetActive(false);
        TimerUI.SetActive(true);
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        TimerUI.GetComponentInChildren<TextMeshProUGUI>().SetText(minutes + ":" + seconds);
    }

    public void SetVsWinner(int playerId)//1 to p1 -------- 2 to p2
    {
        TimerUI.SetActive(false);
        WinnerImage.SetActive(true);

        if(playerId == 1)
        {
            WinnerImage.GetComponent<Image>().sprite = playersSprites[Random.Range(0, 2)];
        }
        else
        {
            WinnerImage.GetComponent<Image>().sprite = playersSprites[Random.Range(2, 4)];
        }
    }

    public void DisableScore()
    {
        TimerUI.SetActive(false);
        WinnerImage.SetActive(false);
    }

    public void Twitter()
    {
        Application.OpenURL("https://twitter.com/Veludito");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
