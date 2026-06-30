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
        GameObject drop = Instantiate(dropPrefab, transform.position, transform.rotation);
        drop.GetComponent<ItemDrop>().Initialize(oreData.dropData);

        Destroy(gameObject);
    }
}