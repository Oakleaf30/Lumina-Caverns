using UnityEngine;

[CreateAssetMenu(fileName = "NewChestData", menuName = "Lumina Caverns/Chest Data")]
public class ChestData : ScriptableObject
{
    public LootTable loot;
    public int itemAmount;
    public Sprite sprite;
    public AnimationClip openAnimation;
}
