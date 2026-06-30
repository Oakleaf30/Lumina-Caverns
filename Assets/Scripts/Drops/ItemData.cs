using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Lumina Caverns/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemId;          // Unique internal ID, e.g., "copper_ingot"
    public string displayName;     // What the player sees, e.g., "Copper Ingot"
    public Sprite icon;
}