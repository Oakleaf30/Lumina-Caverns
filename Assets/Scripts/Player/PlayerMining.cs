using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private float strikeOffset = 0.5f;
    [SerializeField] private float verticalCenterOffset = 0.5f;

    [SerializeField] private EquipmentRegistry registry;

    public PickaxeData pickaxe => GameSession.Instance.runState.pickaxe;
    public int pickaxeIndex => GameSession.Instance.runState.pickaxeIndex;
    public PickaxeTier tier => GameSession.Instance.runState.tier;
    public int tierIndex => GameSession.Instance.runState.tierIndex;

    public int maxPickaxeDurability => tier.maxDurability;
    public int pickaxeDurability;

    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        ApplyPickaxeData();
    }

    private void ApplyPickaxeData()
    {
        GameSession.Instance.runState.pickaxe = registry.pickaxes[pickaxeIndex];
        GameSession.Instance.runState.tier = pickaxe.tiers[tierIndex];
        pickaxeDurability = GameSession.Instance.runState.pickaxeDurability;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= lastSwingTime + swingCooldown && pickaxeDurability > 0 && !anim.GetBool("IsSwimming"))
        {
            SwingPickaxe();
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.ResetTrigger("Pickaxe");
        }
    }

    void SwingPickaxe()
    {
        lastSwingTime = Time.time;

        playerInteraction.SyncAnimatorDirection();

        anim.SetTrigger("Pickaxe");
        playerMovement.DisableMovement();

        onPickaxeSwing.Raise();
    }

    // Called via animation event
    public void DamageNode()
    {
        // 1. Calculate the center origin (waist height instead of feet pivot)
        Vector3 centerOrigin = transform.position + new Vector3(0, verticalCenterOffset, 0);

        // 2. Get player look direction and project the strike position forward smoothly
        Vector2 lookDir = playerMovement != null ? playerInteraction.GetLastDirection() : Vector2.down;
        Vector3 strikeWorldPosition = centerOrigin + (Vector3)(lookDir * strikeOffset);

        // 3. Fluid overlap check (completely detached from the grid)
        Collider2D hit = Physics2D.OverlapCircle(strikeWorldPosition, strikeRadius, resourceLayer);

        if (hit != null && hit.TryGetComponent<OreNode>(out OreNode node))
        {
            node.TakeDamage(pickaxeDamage);
            pickaxeDurability--;

            if (tierIndex == 2) pickaxe.specialAbility?.OnMine(this, node.transform.position);
        }
    }

    // Draws the interactive mining field in the Unity Scene View
    private void OnDrawGizmosSelected()
    {
        // Mirror the exact math used in DamageNode()
        Vector3 centerOrigin = transform.position + new Vector3(0, verticalCenterOffset, 0);

        // Grab look direction safely if the game isn't running yet, fallback to down
        Vector2 lookDir = playerMovement != null ? playerInteraction.GetLastDirection() : Vector2.down;
        Vector3 strikeWorldPosition = centerOrigin + (Vector3)(lookDir * strikeOffset);

        // Draw the look origin (waist point)
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(centerOrigin, 0.05f);

        // Draw the actual hit detection circle
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(strikeWorldPosition, strikeRadius);
    }
}
