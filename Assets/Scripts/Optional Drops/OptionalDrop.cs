using UnityEngine;

[CreateAssetMenu(menuName = "Lumina Caverns/Optional Drop")]
public class OptionalDrop : ScriptableObject
{
    public ItemData dropData;
    [Range(0f, 1f)] public float dropChance;
}