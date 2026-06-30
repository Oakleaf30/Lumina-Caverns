using UnityEngine;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Lumina Caverns/Ore Data")]
public class OreData : ScriptableObject
{
    [Header("Identity")]
    public string oreDisplayName;
    public float hitboxSize;
    public float spaceRequired;

    [Header("Mining Stats")]
    public int maxHitPoints = 3;

    [Header("Drops Configuration")]
    public ItemData dropData;
    public int minDropCount = 1;
    public int maxDropCount = 3;

    [Header("Visuals")]
    public Sprite[] nodeSprites;
}