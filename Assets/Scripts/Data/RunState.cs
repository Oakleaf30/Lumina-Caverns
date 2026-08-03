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

    public ArmourData armour;
    public int armourIndex;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public int currentHealth;
    public int currentFloor;

    public void Initialise(int startingHealth = 100)
    {
        inventory.Clear();
        storage.Clear();
        currentHealth = startingHealth;

        currentFloor = 0;
    }
}