using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameEvent onChestOpen;
    [SerializeField] private GameObject dropPrefab;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ChestData chestData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void InitialiseImmediate(ChestData data)
    {
        chestData = data;
        spriteRenderer.sprite = chestData.sprite;

        AnimatorOverrideController overrideController =
        new AnimatorOverrideController(animator.runtimeAnimatorController);

        overrideController["Placeholder"] = chestData.openAnimation;

        animator.runtimeAnimatorController = overrideController;

        animator.enabled = false;
    }

    private void OnEnable()
    {
        onChestOpen.Subscribe(Open);
    }

    private void OnDisable()
    {
        onChestOpen.Unsubscribe(Open);
    }

    private void Open()
    {
        animator.enabled = true;
    }

    public void SpawnLoot()
    {
        for (int i = 0; i < 3; i++)
        {
            var loot = chestData.loot.GetRandomLoot();

            for (int j = 0; j < loot.amount; j++)
            {
                SpawnDrop(loot.item);
            }
        }
    }

    private void SpawnDrop(ItemData item)
    {
        float outerRadius = 1f;
        float innerRadius = 0.5f;

        Vector2 offset = Random.insideUnitCircle.normalized *
                         Random.Range(innerRadius, outerRadius);
        Vector3 spawnLocation = transform.position + new Vector3(offset.x, offset.y, 0);

        GameObject drop = Instantiate(dropPrefab, spawnLocation, transform.rotation);
        drop.GetComponent<ItemDrop>().Initialize(item);
    }
}
