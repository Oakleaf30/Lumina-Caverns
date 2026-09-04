using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private ItemData itemData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Call this immediately after instantiating the prefab from ANY source
    public void Initialize(ItemData data)
    {
        itemData = data;

        spriteRenderer.sprite = itemData.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (itemData.itemId == "bomb")
                BaseStorage.Current.AddItem(itemData, 1);
            else
                collision.GetComponent<PlayerInventory>().AddItem(itemData, 1);

            Destroy(gameObject);
        }
    }
}