using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private GameEvent onSwordSwing;

    private Animator anim;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    [Header("Sword Setup")]
    [SerializeField] private float swingCooldown = 0.4f;
    [SerializeField] private int swordDamage = 1;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Hitbox Geometry")]
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackOffset = 0.4f; // Distance pushed in front of the player
    [SerializeField] private float knockbackForce = 5f;

    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
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

        // Get directional look vector from your interaction system
        Vector2 lookDir = playerInteraction.GetLastDirection().normalized;
        anim.SetFloat("MoveX", lookDir.x);
        anim.SetFloat("MoveY", lookDir.y);

        anim.SetTrigger("Sword");
        playerMovement.DisableMovement();

        // Broadcast the event to any independent listening systems (UI, Audio, etc.)
        onSwordSwing.Raise();
    }

    void CalculateMeleeHitbox()
    {
        Vector2 lookDirection = playerInteraction.GetLastDirection().normalized;
        Vector2 attackOrigin = (Vector2)transform.position + (lookDirection * attackOffset);
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackOrigin, attackRadius, enemyLayer);

        foreach (Collider2D targetCollider in hitTargets)
        {
            if (targetCollider.TryGetComponent(out EnemyBase enemy))
            {
                // Calculate the angle away from the player
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                Vector2 totalForce = knockbackDir * knockbackForce;

                // Pass the damage AND the force vector
                enemy.TakeDamage(swordDamage, totalForce);
            }
        }
    }

    // Draws the hitbox radius in the Scene View for easy visual tuning
    private void OnDrawGizmosSelected()
    {
        if (playerInteraction == null) playerInteraction = GetComponent<PlayerInteraction>();
        if (playerInteraction == null) return;

        Vector2 lookDir = playerInteraction.GetLastDirection().normalized;
        Vector2 attackOrigin = (Vector2)transform.position + (lookDir * attackOffset);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}