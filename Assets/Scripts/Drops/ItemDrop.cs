using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private ItemData currentItemData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Call this immediately after instantiating the prefab from ANY source
    public void Initialize(ItemData data)
    {
        currentItemData = data;

        spriteRenderer.sprite = currentItemData.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerInventory>().AddItem(currentItemData);

            Destroy(gameObject);
        }
    }
}