using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Lumina Caverns/Ore Data")]
public class OreData : ScriptableObject
{
    [Header("Identity")]
    public string oreDisplayName;
    public float hitboxSize;
    public float spaceRequired;

    [Header("Mining Stats")]
    public int maxHitPoints;

    [Header("Drops Configuration")]
    public ItemData dropData;
    public int minDropCount;
    public int maxDropCount;
    public OptionalDrop[] optionalDrops;

    [Header("Visuals")]
    public Sprite[] nodeSprites;
}