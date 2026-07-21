using UnityEngine;

[CreateAssetMenu(menuName = "Items/Pickaxe")]
public class PickaxeData : ItemData
{
    public ItemData[] gemOptions;
    public PickaxeAbility specialAbility;
    public PickaxeTier[] tiers; // Crude, Refined, Flawless
}

[System.Serializable]
public struct PickaxeTier
{
    public string tierName;
    public int damage;
    public int maxDurability;
    public ItemData costItem;
    public int costAmount;
}

[CreateAssetMenu(menuName = "Items/Armour")]
public class ArmourData : ItemData
{
    public int healthIncrease;
}

[CreateAssetMenu(menuName = "Items/Sword")]
public class SwordData : ItemData
{
    public int damage;
}