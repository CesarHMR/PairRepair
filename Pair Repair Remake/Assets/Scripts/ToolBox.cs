using UnityEngine;

public class ToolBox : MonoBehaviour
{
    public PlayerSkill.tool startTool;
    [HideInInspector]public PlayerSkill.tool currentTool;
    public Sprite[] toolsSprite;


    [SerializeField] Animator anim;
    [SerializeField] private SpriteRenderer sp;

    private void Start()
    {
        GameEvents.OnGameStart.AddListener(ResetTool);
    }

    public void TradeTool(PlayerSkill playerTool)
    {
        PlayerSkill.tool playerOldTool = playerTool.currentTool;
        playerTool.currentTool = currentTool;
        currentTool = playerOldTool;
        sp.sprite = toolsSprite[ChangeSprite()];
        anim.SetTrigger("Trade");
    }

    int ChangeSprite()
    {
        switch (currentTool)
        {
            case PlayerSkill.tool.Wood:
                return 0;
            case PlayerSkill.tool.Hammer:
                return 1;
            case PlayerSkill.tool.Brick:
                return 2;
            case PlayerSkill.tool.Trowel:
                return 3;
            case PlayerSkill.tool.Hand:
                return 4;
        }

        return 4;
    }

    void ResetTool()
    {
        currentTool = startTool;
        sp.sprite = toolsSprite[ChangeSprite()];
    }
}
