using UnityEngine;

[CreateAssetMenu(menuName = "Items/Pickaxe")]
public class PickaxeData : ItemData
{
    public PickaxeTier[] tiers; // Crude, Refined, Flawless
}

[System.Serializable]
public struct PickaxeTier
{
    public string tierName;
    public int damage;
    public int maxDurability;
    public ItemData repairBar;
    public ItemData magicOreCost;
    public ItemData[] gemCost;
    public PickaxeAbility specialAbility;
}

[CreateAssetMenu(menuName = "Items/Armour")]
public class ArmourData : ItemData
{
    public int healthIncrease; //e
}

[CreateAssetMenu(menuName = "Items/Sword")]
public class SwordData : ItemData
{
    public int damage;
}