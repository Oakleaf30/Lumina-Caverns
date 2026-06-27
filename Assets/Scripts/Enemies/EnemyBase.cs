using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // 1. Changed to protected so Slime can read its speed, health, etc.
    [SerializeField] protected EnemyData data;

    protected int currentHealth;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    protected Rigidbody2D rb;

    protected PlayerMovement player;
    protected int roomID;

    protected float knockbackTimer;
    [SerializeField] protected float knockbackDuration;

    // 2. Changed to protected virtual so Slime can run its own Awake code if needed
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // 3. Changed to protected virtual so Slime can have its own movement logic in Update/FixedUpdate
    protected virtual void Update()
    {
        if (player.CurrentRoomID != roomID)
        {
            FreezeEnemy();
        }
        else
        {
            UnfreezeEnemy();
            // Children scripts can handle their custom movement if they override this!
        }

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

    void FreezeEnemy()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    void UnfreezeEnemy()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void InitializeEnemy(int roomID)
    {
        if (data == null) return;

        currentHealth = data.maxHealth;
        spriteRenderer.sprite = data.defaultSprite;
        anim.runtimeAnimatorController = data.animatorController;
        this.roomID = roomID;
    }

    public void TakeDamage(int amount, Vector2 knockbackVector)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

        ApplyKnockback(knockbackVector);
    }

    void ApplyKnockback(Vector2 knockbackVector)
    {
        knockbackTimer = knockbackDuration;

        float forceModifier = 1f - data.knockbackResistance;
        Vector2 finalForce = knockbackVector * forceModifier;

        rb.AddForce(finalForce, ForceMode2D.Impulse);
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