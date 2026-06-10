using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData data; // Assign your custom SO asset here

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        if (data == null) return;

        // Apply data from the ScriptableObject asset
        currentHealth = data.maxHealth;
        spriteRenderer.sprite = data.defaultSprite;
        anim.runtimeAnimatorController = data.animatorController;
    }

    public void TakeDamage(int amount, Vector2 knockbackVector)
    {
        currentHealth -= amount;

        // Flash effects, knockback calculation, etc.

        if (currentHealth <= 0)
        {
            Die();
        }

        // 2. Direct pure physics knockback check
        if (rb != null && data != null && data.knockbackResistance < 1f)
        {
            // Read straight from the SO asset without any local caching variables
            float forceModifier = 1f - data.knockbackResistance;
            Vector2 finalForce = knockbackVector * forceModifier;

            rb.AddForce(finalForce, ForceMode2D.Impulse);
        }
    }

    private void Die()
    {
        // Drop items based on data guidelines
        if (data.dropItemPrefab != null && Random.value <= data.dropChance)
        {
            Instantiate(data.dropItemPrefab, transform.position, Quaternion.identity);
        }

        // Fire a game event to notify rooms/managers an enemy died if needed
        Destroy(gameObject);
    }
}