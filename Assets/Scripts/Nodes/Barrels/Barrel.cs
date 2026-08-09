using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;

    [SerializeField] private CircleCollider2D hitbox;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private BarrelData barrelData;

    private int currentHitPoints;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void InitialiseImmediate(BarrelData data)
    {
        barrelData = data;

        currentHitPoints = barrelData.maxHitPoints;
        spriteRenderer.sprite = barrelData.sprite;
        circleCollider.radius = barrelData.hitboxSize;
        hitbox.radius = barrelData.hitboxSize;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHitPoints -= damageAmount;

        if (currentHitPoints <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        CalculateDrops();
        Destroy(gameObject);
    }

    private void CalculateDrops()
    {
        var loot = barrelData.lootTable.GetRandomLoot();

        for (int i = 0; i < loot.amount; i++)
        {
            SpawnDrop(loot.item);
        }
    }

    private void SpawnDrop(ItemData item)
    {
        Vector2 offset = Random.insideUnitCircle * 0.5f;
        Vector3 spawnLocation = transform.position + new Vector3(offset.x, offset.y, 0);

        GameObject drop = Instantiate(dropPrefab, spawnLocation, transform.rotation);
        drop.GetComponent<ItemDrop>().Initialize(item);
    }
}
