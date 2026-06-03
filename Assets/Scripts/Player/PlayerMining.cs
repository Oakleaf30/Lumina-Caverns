using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private GameEvent onPickaxeSwing;

    private Animator anim;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    [Header("Mining Mechanics")]
    [SerializeField] private float swingCooldown = 0.4f;
    [SerializeField] private float strikeRadius = 0.4f;
    [SerializeField] private LayerMask resourceLayer;
    [SerializeField] private int pickaxeDamage = 1;

    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastSwingTime + swingCooldown && !anim.GetBool("IsSwimming"))
        {
            SwingPickaxe();
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.ResetTrigger("Swing");
        }
    }

    void SwingPickaxe()
    {
        lastSwingTime = Time.time;

        Vector2 lookDir = playerInteraction.GetLastDirection();
        anim.SetFloat("MoveX", lookDir.x);
        anim.SetFloat("MoveY", lookDir.y);

        anim.SetTrigger("Swing");
        playerMovement.DisableMovement();

        onPickaxeSwing.Raise();
    }

    void DamageNode()
    {
        Vector3 targetWorldPosition = playerInteraction.GetTargetCellCenterWorld();
        Collider2D hit = Physics2D.OverlapCircle(targetWorldPosition, strikeRadius, resourceLayer);

        if (hit != null && hit.TryGetComponent<OreNode>(out OreNode node))
        {
            node.TakeDamage(pickaxeDamage);
        }
    }
}