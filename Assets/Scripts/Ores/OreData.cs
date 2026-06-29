using UnityEngine;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Lumina Caverns/Ore Data")]
public class OreData : ScriptableObject
{
    [Header("Identity & Drops")]
    public string primaryItemYieldID;
    public string oreDisplayName;
    public float hitboxSize;
    public float spaceRequired;

    [Header("Mining Stats")]
    public int maxHitPoints = 3;

    [Header("Visuals")]
    public Sprite[] nodeSprites;
}