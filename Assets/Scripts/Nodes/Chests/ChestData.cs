using UnityEngine;

[CreateAssetMenu(fileName = "NewChestData", menuName = "Lumina Caverns/Chest Data")]
public class ChestData : ScriptableObject
{
    public LootTable loot;
    public Sprite sprite;
    public AnimationClip openAnimation;
}
