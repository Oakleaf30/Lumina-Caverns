using UnityEngine;

public enum ItemCategory { Ore, Bar, Gem, MonsterDrop, Misc }

[CreateAssetMenu(fileName = "NewItemData", menuName = "Lumina Caverns/Item Data")]

public class ItemData : ScriptableObject
{
    public string itemId;
    public string displayName;
    public Sprite icon;
    public ItemCategory category;
}