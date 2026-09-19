using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameEvent onSwordSwing;
    [SerializeField] private EquipmentRegistry registry;
    [SerializeField] private GameObject explosionPrefab;

    private Animator anim;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    [Header("Sword Setup")]
    [SerializeField] private float swingCooldown = 0.4f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Hitbox Geometry")]
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackOffset = 0.4f; // Distance pushed in front of the player
    [SerializeField] private float verticalCenterOffset = 0.5f; // NEW: Vertically shifts origin from feet to waist/chest
    [SerializeField] private float knockbackForce = 5f;

    private SwordData sword => GameSession.Instance.runState.sword;

    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        GameSession.Instance.runState.sword = registry.swords[GameSession.Instance.runState.swordIndex];
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= lastSwingTime + swingCooldown && !anim.GetBool("IsSwimming"))
        {
            SwingSword();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.ResetTrigger("Sword");
        }
    }

    void SwingSword()
    {
        lastSwingTime = Time.time;

        playerInteraction.SyncAnimatorDirection();

        anim.SetTrigger("Sword");
        playerMovement.DisableMovement();

        // Broadcast the event to any independent listening systems (UI, Audio, etc.)
        onSwordSwing.Raise();
    }

    // Call this from your Animation Event timeline at the peak of your swing!
    public void CalculateMeleeHitbox()
    {
        Vector2 lookDirection = playerInteraction.GetLastDirection().normalized;

        // Fix: Establish the pivot core offset from the feet up to the waist/chest before pushing outward
        Vector2 centerOrigin = (Vector2)transform.position + new Vector2(0f, verticalCenterOffset);
        Vector2 attackOrigin = centerOrigin + (lookDirection * attackOffset);

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackOrigin, attackRadius, enemyLayer);

        foreach (Collider2D targetCollider in hitTargets)
        {
            if (targetCollider.TryGetComponent(out EnemyBase enemy))
            {
                Vector2 knockbackDir = ((Vector2)enemy.transform.position - centerOrigin).normalized;

                HitInfo hit = new HitInfo
                {
                    damage = sword.damage,
                    knockback = knockbackDir * knockbackForce
                };

                if (GameSession.Instance.runState.TryGetEnchant(EnchantSlot.Sword, "sword_stun", out var stun) && Random.value <= stun.procChance)
                {
                    hit.isStun = true;
                    hit.stunDuration = stun.value;
                }

                bool wasAlive = enemy.currentHealth > 0;
                enemy.TakeDamage(sword.damage, hit);

                if (wasAlive && enemy.currentHealth <= 0
                    && GameSession.Instance.runState.TryGetEnchant(EnchantSlot.Sword, "demolitionist", out var explode)
                    && Random.value <= explode.procChance)
                {
                    var bomb = Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
                    bomb.GetComponent<Bomb>().Init(0, 1f, 0, false);
                }
            }

            if (targetCollider.TryGetComponent(out Barrel barrel))
            {
                barrel.TakeDamage(sword.damage);
            }
        }
    }

    // Draws the hitbox radius in the Scene View for easy visual tuning
    private void OnDrawGizmosSelected()
    {
        if (playerInteraction == null) return;

        Vector2 lookDir = playerInteraction.GetLastDirection().normalized;

        // Mirror the updated logic here so the visual representation in the inspector is 100% accurate
        Vector2 centerOrigin = (Vector2)transform.position + new Vector2(0f, verticalCenterOffset);
        Vector2 attackOrigin = centerOrigin + (lookDir * attackOffset);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}

public struct HitInfo
{
    public float damage;
    public Vector2 knockback;
    public bool isStun;
    public float stunDuration;
}