using UnityEngine;

[CreateAssetMenu(fileName = "NewBarrelData", menuName = "Lumina Caverns/Barrel Data")]
public class BarrelData : ScriptableObject
{
    [Header("Identity")]
    public float hitboxSize;
    public float spaceRequired;

    [Header("Stats")]
    public int maxHitPoints;

    [Header("Drops Configuration")]
    public LootTable lootTable;

    [Header("Visuals")]
    public Sprite sprite;
}