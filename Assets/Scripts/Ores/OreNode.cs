using UnityEngine;

public class OreNode : MonoBehaviour
{
    [SerializeField] private OreData oreData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Global event architecture reference
    [SerializeField] private GameEvent onOreMined;

    private int currentHitPoints;

    private void Start()
    {
        InitializeNode();
    }

    public void Initialize(OreData data)
    {
        oreData = data;
        InitializeNode();
    }

    private void InitializeNode()
    {
        if (oreData == null) return;

        currentHitPoints = oreData.maxHitPoints;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = oreData.nodeSprite;
        }
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