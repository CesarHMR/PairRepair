using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectScript : MonoBehaviour
{
    public GameObject playButton, tutorialButton, tutorialComponent, closeTutoButton;
    bool on = false;
    public void Tutorial()
    {
        on = !on;
        closeTutoButton.SetActive(on);
        tutorialComponent.SetActive(on);

        if(on)
            EventSystem.current.SetSelectedGameObject(closeTutoButton);
        else
            EventSystem.current.SetSelectedGameObject(tutorialButton);
    }

    private void Awake()
    {
        GameEvents.OnGameEnd.AddListener(StartButton);
        GameEvents.OnGameStart.AddListener(NoButton);
    }
    void StartButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    void NoButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
