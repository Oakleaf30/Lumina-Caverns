using UnityEngine;

public class OreNode : MonoBehaviour
{
    [SerializeField] private GameEvent onOreMined;
    [SerializeField] private GameObject dropPrefab;

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private OreData oreData;

    private int currentHitPoints;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void InitialiseImmediate(OreData data)
    {
        oreData = data;

        currentHitPoints = oreData.maxHitPoints;
        spriteRenderer.sprite = oreData.nodeSprites[Random.Range(0, oreData.nodeSprites.Length)];
        circleCollider.radius = oreData.hitboxSize;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHitPoints -= damageAmount;

        if (currentHitPoints <= 0)
        {
            BreakNode();
        }
    }

    private void BreakNode()
    {
        int amount = Random.Range(oreData.minDropCount, oreData.maxDropCount);

        for (int i  = 0; i < amount; i++ )
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnLocation = transform.position + new Vector3(offset.x, offset.y, 0);

            GameObject drop = Instantiate(dropPrefab, spawnLocation, transform.rotation);
            drop.GetComponent<ItemDrop>().Initialize(oreData.dropData);
        }

        Destroy(gameObject);
    }

    
}