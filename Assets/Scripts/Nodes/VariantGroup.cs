using UnityEngine;

[System.Serializable]
public struct SpawnPool
{
    [Tooltip("The readable name of this resource group (e.g., Copper)")]
    public string resourceName;

    [Header("Size Variants")]
    public OreData smallVariant;
    public OreData largeVariant;
    public OreData guaranteedVariant;

    [Header("Spawning Weights")]
    [Range(0f, 1f), Tooltip("Relative chance to spawn this specific ore type within a biome.")]
    public float selectionWeight;

    [Range(0f, 1f), Tooltip("When this ore is selected, the chance it spawns as a large node vs small.")]
    public float largeSizeChance;
}