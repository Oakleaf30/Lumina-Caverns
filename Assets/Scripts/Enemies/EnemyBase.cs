using TreeEditor;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // 1. Changed to protected so Slime can read its speed, health, etc.
    [SerializeField] protected EnemyData data;
    [SerializeField] protected GameObject dropPrefab;

    [SerializeField] protected Vector3Event onEnemyLadder;

    protected int currentHealth;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    protected Rigidbody2D rb;

    protected PlayerMovement player;
    protected int roomID;

    protected bool frozen = false;

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
            frozen = true;
        }
        else
        {
            frozen = false;
        }
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
        float forceModifier = 1f - data.knockbackResistance;
        Vector2 finalForce = knockbackVector * forceModifier;

        rb.AddForce(finalForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (Random.value < data.dropChance)
        {
            SpawnDrop(data.dropData);
        }

        foreach (var optional in data.optionalDrops)
        {
            if (Random.value < optional.dropChance)
                SpawnDrop(optional.dropData);
        }

        onEnemyLadder.Raise(transform.position);
        Destroy(gameObject);
    }

    private void SpawnDrop(ItemData item)
    {
        Vector2 offset = Random.insideUnitCircle * 0.5f;
        Vector3 spawnLocation = transform.position + new Vector3(offset.x, offset.y, 0);

        GameObject drop = Instantiate(dropPrefab, spawnLocation, transform.rotation);
        drop.GetComponent<ItemDrop>().Initialize(item);
    }
}