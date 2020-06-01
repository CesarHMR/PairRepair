using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGoldenFix : MonoBehaviour
{
    GoldenFix gf;
    PlayerInput pi;

    private void Start()
    {
        gf = FindObjectOfType<GoldenFix>();
        pi = GetComponent<PlayerInput>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Golden")
        {
            gf.RepairAll(pi.id);
        }
    }
}
