using UnityEngine;

public class OreNode : MonoBehaviour
{
    public OreData oreData;
    [SerializeField] private GameEvent onOreMined;

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private int currentHitPoints;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void InitialiseImmediate()
    {
        if (oreData == null) return;

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
        // 1. Handle own destruction / spawn visual particles locally
        Debug.Log($"{oreData.oreDisplayName} broken!");

        // 2. Fire decoupled event passing details to UI or Inventory
        if (onOreMined != null)
        {
            // You can pass the primaryItemYieldID through your event system
            //onOreMined.Raise(oreData.primaryItemYieldID);
        }

        Destroy(gameObject);
    }
}