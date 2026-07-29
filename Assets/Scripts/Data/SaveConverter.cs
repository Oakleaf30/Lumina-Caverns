public static class SaveConverter
{
    public static SaveData ToSaveData(RunState runState)
    {
        var data = new SaveData
        {
            pickaxeIndex = runState.pickaxeIndex,
            pickaxeTier = runState.tierIndex,
            pickaxeDurability = runState.pickaxeDurability,
        };

        foreach (var kvp in runState.storage)
            data.storage.Add(new ItemCountEntry { itemId = kvp.Key.itemId, count = kvp.Value });

        return data;
    }

    public static void ApplyToRunState(SaveData data, RunState runState)
    {
        runState.pickaxeIndex = data.pickaxeIndex;
        runState.tierIndex = data.pickaxeTier;
        runState.pickaxeDurability = data.pickaxeDurability;

        runState.storage.Clear();
        foreach (var entry in data.storage)
        {
            var item = ItemDatabase.GetById(entry.itemId);
            if (item != null) runState.storage[item] = entry.count;
        }
    }

    public static TempData ToTempSave(RunState runState)
    {
        var data = new TempData
        {
            currentFloor = runState.currentFloor,
            currentHealth = runState.currentHealth,
            pickaxeDurability = runState.pickaxeDurability,
        };

        foreach (var kvp in runState.inventory)
            data.inventory.Add(new ItemCountEntry { itemId = kvp.Key.itemId, count = kvp.Value });

        return data;
    }

    public static void ApplyTemp(TempData data, RunState runState)
    {
        runState.currentFloor = data.currentFloor;
        runState.currentHealth = data.currentHealth;
        runState.pickaxeDurability = data.pickaxeDurability;

        runState.inventory.Clear();
        foreach (var entry in data.inventory)
        {
            var item = ItemDatabase.GetById(entry.itemId);
            if (item != null) runState.inventory[item] = entry.count;
        }
    }
}