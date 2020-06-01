using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    public int id;//player indentifier
    [SerializeField] private float speed;//player speed
    public bool canMove = false;//alowed to move or not - main menu exemple

    float horizontal;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public AudioManager am;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        /////////////////////////////////////////////////
        GameEvents.OnGameStart.AddListener(SetCanMoveTrue);
        GameEvents.OnGameEnd.AddListener(SetCanMoveFalse);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal" + id);

        if (canMove)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    public bool DownInput() => (((Input.GetButtonDown("Down" + id)) || (Input.GetButtonDown("Jump" + id) && Input.GetAxisRaw("Vertical" + id) < 0)) && canMove) ;

    public bool JumpInput() => ((Input.GetButtonDown("Jump" + id) && Input.GetAxisRaw("Vertical" + id) >= 0) && canMove);

    public bool InteractInput() => ((Input.GetButtonDown("Use" + id)) && canMove);

    void SetCanMoveTrue()
    {
        canMove = true;
    }

    void SetCanMoveFalse()
    {
        canMove = false;
    }
}
