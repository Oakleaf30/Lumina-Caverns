using UnityEngine;

[System.Serializable]
public struct ItemAmount
{
    public ItemData item;
    public int amount;
}

[System.Serializable]
public struct LootItem
{
    public ItemAmount itemAmount;
    public int maxAmount;
    [Range(0f, 1f)] public float chance;
}

[CreateAssetMenu(fileName = "LootTable", menuName = "Lumina Caverns/Loot Table")]
public class LootTable : ScriptableObject
{
    public LootItem[] lootItems;

    public ItemData pityTargetItem;
    public ItemAmount pityReward;
    public int pityThreshold = 6;

    public ItemAmount GetRandomLoot()
    {
        float totalWeight = 0f;
        foreach (var item in lootItems) totalWeight += item.chance;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var item in lootItems)
        {
            cumulative += item.chance;
            if (roll <= cumulative)
                return RollAmount(item);
        }

        // Fallback in case of floating point rounding at the boundary
        return RollAmount(lootItems[lootItems.Length - 1]);
    }

    private ItemAmount RollAmount(LootItem lootItem)
    {
        int finalAmount = lootItem.maxAmount > lootItem.itemAmount.amount
            ? Random.Range(lootItem.itemAmount.amount, lootItem.maxAmount + 1)
            : lootItem.itemAmount.amount;

        return new ItemAmount
        {
            item = lootItem.itemAmount.item,
            amount = finalAmount
        };
    }
}