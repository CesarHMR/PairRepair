using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WallSprite : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite woodBroken;
    [SerializeField] private Sprite woodRepaired;
    [SerializeField] private Sprite brickBroken;
    [SerializeField] private Sprite brickRepaired;
    [Space(5)]
    [Header("Particles")]
    [SerializeField] private GameObject cleanWallParticles;
    [SerializeField] private GameObject woodBrokenParticles;
    [SerializeField] private GameObject woodRepairedParticles;
    [SerializeField] private GameObject brickBrokenParticles;
    [SerializeField] private GameObject brickRepairedParticles;

    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.clear;//Set inicial color invisible
        ////////////////////////////////////
        GameEvents.OnGameStart.AddListener(CleanSprite);
    }

    public void TradeSprite(WallMaterial.wallState state, WallMaterial.material material)//if is broken, fix it (wood and brick)
    {
        GameObject particlesToEmit = new GameObject();
        switch (state)
        {
            case WallMaterial.wallState.broken:

                sr.sprite = material == WallMaterial.material.Wood ? woodBroken : brickBroken;
                sr.color = Color.white;
                particlesToEmit = material == WallMaterial.material.Wood ? woodBrokenParticles : brickBrokenParticles;
                Instantiate(particlesToEmit, transform.position, Quaternion.identity);

                break;

            case WallMaterial.wallState.repaired:

                sr.sprite = material == WallMaterial.material.Wood ? woodRepaired : brickRepaired;
                sr.color = Color.white;
                particlesToEmit = material == WallMaterial.material.Wood ? woodRepairedParticles : brickRepairedParticles;
                Instantiate(particlesToEmit, transform.position, Quaternion.identity);

                break;

            case WallMaterial.wallState.clean:

                CleanSprite();

                break;
        }
    }

    void CleanSprite()
    {
        if(sr.color != Color.clear)
        {
            sr.color = Color.clear;
            Instantiate(cleanWallParticles, transform.position, Quaternion.identity);
        }
    }


}
