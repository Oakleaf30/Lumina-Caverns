using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public readonly struct InventorySlot
{
    public readonly ItemData item;
    public readonly int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class ItemContainer : MonoBehaviour
{
    protected Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    public virtual void AddItem(ItemData item, int amount)
    {
        if (items.ContainsKey(item))
            items[item] += amount;
        else
            items[item] = amount;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (!items.ContainsKey(item) || items[item] < amount) return false;

        items[item] -= amount;
        if (items[item] <= 0) items.Remove(item);
        return true;
    }

    public int GetQuantity(ItemData item) => items.TryGetValue(item, out int q) ? q : 0;

    public List<InventorySlot> GetItemsByCategory(ItemCategory category)
    {
        return items
            .Where(kv => kv.Key.category == category)
            .Select(kv => new InventorySlot(kv.Key, kv.Value))
            .ToList();
    }

    public Dictionary<ItemData, int> GetAllItems() => items;
    public void ClearAll() => items.Clear();
}