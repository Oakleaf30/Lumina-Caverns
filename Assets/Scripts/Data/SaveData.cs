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

    public int armourIndex;
    public int swordIndex;
}