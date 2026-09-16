public static class SaveConverter
{
    public static SaveData ToSaveData(RunState runState)
    {
        var data = new SaveData
        {
            pickaxeIndex = runState.pickaxeIndex,
            pickaxeTier = runState.tierIndex,
            pickaxeDurability = runState.pickaxeDurability,

            geodePity = runState.geodePity,
        };

        foreach (var kvp in runState.storage)
            data.storage.Add(new ItemCountEntry { itemId = kvp.Key.itemId, count = kvp.Value });

        foreach (var kvp in runState.equippedEnchants)
        {
            data.enchants.Add(new EnchantSaveEntry
            {
                slot = kvp.Key,
                enchantId = kvp.Value.data.enchantId,
                runsRemaining = kvp.Value.runsRemaining
            });
        }

        return data;
    }

    public static void ApplyToRunState(SaveData data, RunState runState)
    {
        runState.pickaxeIndex = data.pickaxeIndex;
        runState.tierIndex = data.pickaxeTier;
        runState.pickaxeDurability = data.pickaxeDurability;

        runState.armourIndex = data.armourIndex;
        runState.swordIndex = data.swordIndex;

        runState.geodePity = data.geodePity;

        runState.storage.Clear();
        foreach (var entry in data.storage)
        {
            var item = ItemDatabase.GetById(entry.itemId);
            if (item != null) runState.storage[item] = entry.count;
        }

        runState.equippedEnchants.Clear();
        foreach (var entry in data.enchants)
        {
            var enchantData = EnchantDatabase.GetById(entry.enchantId);
            if (enchantData != null)
            {
                runState.equippedEnchants[entry.slot] = new ActiveEnchant
                {
                    data = enchantData,
                    runsRemaining = entry.runsRemaining
                };
            }
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