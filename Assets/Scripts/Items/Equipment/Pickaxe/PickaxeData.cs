using UnityEngine;

[CreateAssetMenu(menuName = "Items/Pickaxe")]
public class PickaxeData : ItemData
{
    [Header("Pickaxe Settings")]
    public int damage;
    public int durabilityPerBar;
    public PickaxeAbility specialAbility;
    public ItemData[] gemOptions;

    [Space(20)]
    public PickaxeTier[] tiers; // Crude, Refined, Flawless
}

[System.Serializable]
public struct PickaxeTier
{
    public string tierName;
    public int maxDurability;
    public ItemData costItem;
    public int costAmount;
}