using System;
using System.Collections.Generic;

[Serializable]
public class TempData
{
    public List<ItemCountEntry> inventory = new();
    public int currentFloor;
    public int currentHealth;
    public int pickaxeDurability;
}