using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Biome", menuName = "Lumina Caverns/Biome Data")]
public class BiomeData : ScriptableObject
{
    [Header("Identity")]
    public string biomeName;

    [Header("Resource Pools")]
    public List<SpawnPool> spawnPools;
    public int activeAnchors;

    [Header("Enemies")]
    public List<GameObject> enemyPool;
    public List<EnemyNumber> enemyCount;
}