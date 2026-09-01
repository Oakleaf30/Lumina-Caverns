using UnityEngine;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Lumina Caverns/Ore Data")]
public class OreData : ScriptableObject
{
    [Header("Identity")]
    public float hitboxSize;
    public float spaceRequired;

    [Header("Mining Stats")]
    public int maxHitPoints;

    [Header("Drops Configuration")]
    public ItemData dropData;
    public int minDropCount;
    public int maxDropCount;
    public OptionalDrop[] optionalDrops = new OptionalDrop[0];

    [Header("Visuals")]
    public Sprite[] nodeSprites;
}