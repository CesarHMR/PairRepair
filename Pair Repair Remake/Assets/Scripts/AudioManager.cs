using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup master;

    private void Awake()
    {
        foreach (Sound item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            item.source.volume = item.volume;
            item.source.pitch = item.pitch;
            item.source.loop = item.loop;
            item.source.outputAudioMixerGroup = master;
        }
    }

    private void Start()
    {
        Play("Morning");
        GameEvents.OnGameStart.AddListener(GameStart);
        GameEvents.OnGameEnd.AddListener(EndGame);
    }

    void GameStart()
    {
        Play("GameMusic");
        Sound s = Array.Find(sounds, sounds => sounds.name == "GameMusic");
        s.source.volume = 0.2f;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }

    public void FadeInOut(string name, float mult, bool fadeOut, float maxVolume)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (fadeOut)
        {
            StartCoroutine(FadeOut(mult, s));
        }
        else
        {
            StartCoroutine(FadeIn(mult, s, maxVolume));
        }
    }

    IEnumerator FadeOut(float mult, Sound s)
    {
        while (s.source.volume > 0)
        {
            s.source.volume -= Time.deltaTime * mult;
            yield return new WaitForFixedUpdate();
        }
        if(s.source.volume <= 0)
        {
            s.source.Stop();
        }
    }

    IEnumerator FadeIn(float mult, Sound s, float maxVolume)
    {
        while (s.source.volume < maxVolume)
        {
            s.source.volume += Time.deltaTime * mult;
            yield return new WaitForFixedUpdate();
        }
    }

    void EndGame()
    {
        FadeInOut("GameMusic", 0.2f, true, 1);
        Play("Morning");
        FadeInOut("Morning", 0.2f, false, 0.46f);
        Play("Whoosh");
    }
}
