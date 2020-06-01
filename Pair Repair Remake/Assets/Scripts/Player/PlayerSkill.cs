using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public enum tool { Wood, Brick, Hammer, Trowel, Hand};

    public tool currentTool;
    [SerializeField] private LayerMask wallLayer, toolBoxLayer;
    [SerializeField] private float checkRadius;

    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        ///////////////////////
        GameEvents.OnGameStart.AddListener(ResetTool);
    }

    private void Update()
    {
        bool wallRange = Physics2D.OverlapCircle(transform.position, checkRadius, wallLayer);
        bool toolBoxRange = Physics2D.OverlapCircle(transform.position, checkRadius, toolBoxLayer);

        if (playerInput.InteractInput())
        {
            if (wallRange)
            {
                GetClosestWall();
            }
            else if (toolBoxRange)
            {
                ToolBox thisToolBox = Physics2D.OverlapCircle(transform.position, checkRadius, toolBoxLayer).GetComponent<ToolBox>();
                thisToolBox.TradeTool(this);
                string soundName = playerInput.id == 0 ? "Click" : "Hover";//Diferent sounds to player 1 e 2
                playerInput.am.Play(soundName);
            }
        }
    }

    void GetClosestWall()
    {
        Collider2D[] theseWalls = Physics2D.OverlapCircleAll(transform.position, checkRadius, wallLayer);
        Collider2D thisWall = theseWalls[0];

        for(int i = 0; i < theseWalls.Length; i++)
        {
            if(Vector2.Distance(transform.position, theseWalls[i].transform.position) < Vector2.Distance(transform.position, thisWall.transform.position))
            {
                thisWall = theseWalls[i];
            }
        }

        WallMaterial wallMaterialReference = thisWall.GetComponent<WallMaterial>();

        if (CheckToolMaterial(wallMaterialReference))
        {
            wallMaterialReference.Interact();
        }
    }

    bool CheckToolMaterial(WallMaterial wallReference)
    {
        bool isValid = false;

        switch (wallReference.thisMaterial)
        {
            case WallMaterial.material.Brick:

                if (currentTool == tool.Trowel && wallReference.thisState == WallMaterial.wallState.broken)
                {
                    isValid = true;
                }
                else if (currentTool == tool.Brick && wallReference.thisState == WallMaterial.wallState.repaired)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }

                break;

            case WallMaterial.material.Wood:

                if (currentTool == tool.Wood && wallReference.thisState == WallMaterial.wallState.broken)
                {
                    isValid = true;
                }
                else if (currentTool == tool.Hammer && wallReference.thisState == WallMaterial.wallState.repaired)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }

                break;
        }

        return isValid;
    }

    void ResetTool()
    {
        currentTool = tool.Hand;
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
*/
}
