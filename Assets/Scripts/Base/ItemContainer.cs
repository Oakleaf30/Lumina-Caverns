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
    [SerializeField] private GameEvent onPotionCountChanged;
    [SerializeField] private GameEvent onBombCountChanged;

    protected Dictionary<ItemData, int> items;

    public virtual void AddItem(ItemData item, int amount)
    {
        if (items.ContainsKey(item))
            items[item] += amount;
        else
            items[item] = amount;

        if (item.itemId == "potion")
            onPotionCountChanged.Raise();

        if (item.itemId == "bomb")
            onBombCountChanged.Raise();
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (item.itemId == "monster_drops")
        {
            int available = 0;
            foreach (var kvp in items)
                if (kvp.Key.category == ItemCategory.MonsterDrop)
                    available += kvp.Value;

            if (available < amount) return false;

            int remaining = amount;
            foreach (var kvp in new List<KeyValuePair<ItemData, int>>(items))
            {
                if (kvp.Key.category != ItemCategory.MonsterDrop) continue;
                int take = Mathf.Min(remaining, kvp.Value);
                items[kvp.Key] -= take;
                if (items[kvp.Key] <= 0) items.Remove(kvp.Key);
                remaining -= take;
                if (remaining <= 0) break;
            }
            return true;
        }

        if (!items.ContainsKey(item) || items[item] < amount) return false;

        items[item] -= amount;
        if (items[item] <= 0) items.Remove(item);

        if (item.itemId == "potion")
            onPotionCountChanged.Raise();

        if (item.itemId == "bomb")
            onBombCountChanged.Raise();

        return true;
    }

    public int GetQuantity(ItemData item)
    {
        if (item.itemId == "monster_drops")
        {
            int total = 0;
            foreach (var kvp in items)
                if (kvp.Key.category == ItemCategory.MonsterDrop)
                    total += kvp.Value;
            return total;
        }

        return items.TryGetValue(item, out int q) ? q : 0;
    }

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