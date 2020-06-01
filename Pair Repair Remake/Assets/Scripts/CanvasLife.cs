using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasLife : MonoBehaviour
{
    public Slider blueSlider, redSlider;
    int lifeBlue = 35, lifeRed = 35;

    public GameObject pauseReference;

    public GameObject home;
    public Image pauseButton;
    public Sprite[] pauseImages;

    public AudioMixer master;

    BuildingManager bm;
    Paralax pl;

    private void Awake()
    {
        bm = FindObjectOfType<BuildingManager>();
        pl = FindObjectOfType<Paralax>();
        GameEvents.OnGameStart.AddListener(SetLifeImages);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void ChangeLifeBlue(int amount)
    {
        lifeBlue += amount;
        SetRealTimeLife(blueSlider, lifeBlue);
    }

    public void ChangeLifeRed(int amount)
    {
        lifeRed += amount;
        SetRealTimeLife(redSlider, lifeRed);
    }

    void SetLifeImages()
    {
        lifeBlue = 35;
        lifeRed = 35;
        SetRealTimeLife(redSlider, lifeRed);
        SetRealTimeLife(blueSlider, lifeBlue);
    }

    void SetRealTimeLife(Slider s, int life)
    {
        s.value = life;
    }

    public static bool paused = false;
    float beforeVolume;
    public void Pause()
    {
        if (!BuildingManager.gameRunning)
            return;

        if (paused)
        {
            EventSystem.current.SetSelectedGameObject(null);
            master.SetFloat("Volume", beforeVolume);
            paused = false;
            Time.timeScale = 1f;
            pauseButton.sprite = pauseImages[0];
            home.SetActive(false);

            PlayerAnimation.paused = false;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseReference);
            master.GetFloat("Volume", out beforeVolume);
            master.SetFloat("Volume", -40f);
            paused = true;
            Time.timeScale = 0;
            pauseButton.sprite = pauseImages[1];
            home.SetActive(true);

            PlayerAnimation.paused = true;
        }
    }

    public void Home()
    {
        master.SetFloat("Volume", beforeVolume);

        Time.timeScale = 1f;
        paused = false;
        PlayerAnimation.paused = false;
        home.SetActive(false);
        pl.DisableScore();
        GameEvents.OnGameEnd.Invoke();
    }
}
