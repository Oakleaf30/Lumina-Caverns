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

    BaseStorage Storage => BaseStorage.Current;
}