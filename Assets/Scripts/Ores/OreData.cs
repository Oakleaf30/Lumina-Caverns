using UnityEngine;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Lumina Caverns/Ore Data")]
public class OreData : ScriptableObject
{
    [Header("Identity & Drops")]
    public string primaryItemYieldID; // e.g., "biome_1_scrap"
    public string oreDisplayName;      // e.g., "Rough Copper Crags"

    [Header("Mining Stats")]
    public int maxHitPoints = 3;

    [Header("Visuals")]
    public Sprite nodeSprite;
    // You could also add visual particles or sparkle effects here
}