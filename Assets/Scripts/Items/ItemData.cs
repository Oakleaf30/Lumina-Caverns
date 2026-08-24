using UnityEngine;

public enum ItemCategory { Ore, Gem, MonsterDrop, Misc, Pickaxe, Sword, Armour }

[CreateAssetMenu(fileName = "NewItemData", menuName = "Lumina Caverns/Item Data")]

public class ItemData : ScriptableObject
{
    public string itemId;
    public string displayName;
    public Sprite icon;
    public ItemCategory category;
}

public enum EquipmentCategory { Pickaxe, Sword, Armour }
public abstract class EquipmentData : ItemData
{
    [Header("Equipment Settings")]
    public ItemData costItem;
    public int costAmount;
}