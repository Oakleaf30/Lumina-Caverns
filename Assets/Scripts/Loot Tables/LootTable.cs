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
    [Range(0f, 1f)] public float chance;
}

[CreateAssetMenu(fileName = "LootTable", menuName = "Lumina Caverns/Loot Table")]
public class LootTable : ScriptableObject
{
    public LootItem[] lootItems;

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
                return item.itemAmount;
        }

        // Fallback in case of floating point rounding at the boundary
        return lootItems[lootItems.Length - 1].itemAmount;
    }
}