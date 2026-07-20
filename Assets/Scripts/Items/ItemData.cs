using UnityEngine;

public enum ItemCategory { Ore, Gem, MonsterDrop, Misc, Equipment }

[CreateAssetMenu(fileName = "NewItemData", menuName = "Lumina Caverns/Item Data")]

public class ItemData : ScriptableObject
{
    public string itemId;
    public string displayName;
    public Sprite icon;
    public ItemCategory category;
}