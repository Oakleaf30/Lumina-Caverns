using System;
using System.Collections.Generic;

[Serializable]
public class ItemCountEntry
{
    public string itemId;
    public int count;
}

[Serializable]
public class SaveData
{
    public List<ItemCountEntry> storage = new();
    public int pickaxeIndex;
    public int pickaxeTier;
    public int pickaxeDurability;
    public string pickaxeEnchantId;
    public int pickaxeEnchantCounter;

    public int armourIndex;
    public string armourEnchantId;
    public int armourEnchantCounter;

    public int swordIndex;
    public string swordEnchantId;
    public int swordEnchantCounter;

    public int geodePity;
}