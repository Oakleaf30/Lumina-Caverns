using UnityEngine;

[System.Serializable]
public struct EnemyNumber
{
    public int enemyNumber;

    [Range(0f, 1f)]
    public float spawnChance;
}