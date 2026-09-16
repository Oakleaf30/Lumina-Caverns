using System.Collections.Generic;

[System.Serializable]
public class RunState
{
    public Dictionary<ItemData, int> storage = new Dictionary<ItemData, int>();
    
    public int pickaxeDurability;
    public PickaxeData pickaxe;
    public int pickaxeIndex;
    public PickaxeTier tier;
    public int tierIndex;
    public int durabilityPerBar;
    public EnchantData pickaxeEnchant;
    public int pickaxeEnchantCounter;

    public ArmourData armour;
    public int armourIndex;
    public EnchantData armourEnchant;
    public int armourEnchantCounter;

    public SwordData sword;
    public int swordIndex;
    public EnchantData swordEnchant;
    public int swordEnchantCounter;

    public Dictionary<EnchantSlot, ActiveEnchant> equippedEnchants = new();

    public int potionCount => Storage.GetQuantity("potion");
    public int bombCount => Storage.GetQuantity("bomb");
    public bool amuletActive => Storage.GetQuantity("amulet") == 1;

    public int geodePity;


    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public int currentHealth;
    public int currentFloor;
    public int cauldronFloorTracker;

    public void Initialise()
    {
        inventory.Clear();
        storage.Clear();

        currentFloor = 0;
    }

    public bool TryGetEnchant(EnchantSlot slot, string enchantId, out EnchantData data)
    {
        if (equippedEnchants.TryGetValue(slot, out var active) && active.data.enchantId == enchantId)
        {
            data = active.data;
            return true;
        }

        data = null;
        return false;
    }

    public void OnRunEnded()
    {
        var expiredSlots = new List<EnchantSlot>();

        foreach (var kvp in equippedEnchants)
        {
            kvp.Value.runsRemaining--;
            if (kvp.Value.runsRemaining <= 0)
                expiredSlots.Add(kvp.Key);
        }

        foreach (var slot in expiredSlots)
            equippedEnchants.Remove(slot);
    }

    BaseStorage Storage => BaseStorage.Current;
}