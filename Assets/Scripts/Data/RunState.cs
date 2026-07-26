using System.Collections.Generic;

[System.Serializable]
public class RunState
{
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, int> storage = new Dictionary<ItemData, int>();
    public int currentHealth;
    public int pickaxeDurability;
    public int currentFloor;

    public PickaxeData pickaxe;
    public int pickaxeIndex;
    public PickaxeTier tier;
    public int tierIndex;

    public void ResetForNewRun(int startingHealth = 3, int startingDurability = 100)
    {
        inventory.Clear();
        storage.Clear();
        currentHealth = startingHealth;
        pickaxeDurability = startingDurability;
        currentFloor = 0;
    }
}