using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/New Effect")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;
    public float duration;

    [Header("Movement Modifiers")]
    public float speedMultiplier = 1.0f; // 0.5f would mean 50% slowed

    [Header("Damage Over Time")]
    public int damagePerTick;
    public float tickInterval;
}