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
    public List<ItemCountEntry> inventory = new();
    public int pickaxeTier;
    public float currentHealth;
    public float pickaxeDurability;
    public int currentFloor;
}