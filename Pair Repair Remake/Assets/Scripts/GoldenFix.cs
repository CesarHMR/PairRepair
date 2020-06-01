using UnityEngine;

public class GoldenFix : MonoBehaviour
{
    public Sprite[] goldenSprites;
    bool fixBrick;//trowel or hammer

    public SpriteRenderer sr;//this Sprite Renderer
    public Animator anim;
    BuildingManager bm;

    float time;
    bool used;
    bool active;
    private void Start()
    {
        bm = FindObjectOfType<BuildingManager>();
        time = 60;
        used = true;
        GameEvents.OnGameEnd.AddListener(Desappear);
        GameEvents.OnGameStart.AddListener(ResetTrigger);
    }
    private void Update()
    {
        if (!BuildingManager.gameRunning)
            return;

        if(time <= 0)
        {
            if (used)
            {
                Appear();
                used = false;
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }

    void ResetTrigger()
    {
        anim.ResetTrigger("Desappear");
    }


    private void Appear()
    {
        active = true;
        int rand = Random.Range(0, 2);//trowel or hammer

        switch (rand)
        {
            case 0:
                sr.sprite = goldenSprites[0];
                fixBrick = true;
                break;
            case 1:
                sr.sprite = goldenSprites[1];
                fixBrick = false;
                break;
        }
        anim.SetTrigger("Appear");
    }

    public void RepairAll(int playerId)
    {
        if (active)
        {
            active = false;
            anim.SetTrigger("Desappear");
            time = 30;
            used = true;
            bm.GoldenFix(playerId, fixBrick);
        }
    }

    void Desappear()
    {
        anim.SetTrigger("Desappear");
        time = 60;
        active = false;
        used = true;
    }
}
