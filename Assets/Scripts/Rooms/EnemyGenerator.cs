using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private TileBase spawnMarker;

    private List<GameObject> enemies;
    private List<EnemyNumber> enemyNumbers;

    private int totalEnemies = 0;
    [SerializeField] private LadderTile ladder;

    private void Setup(BiomeData biome)
    {
        totalEnemies = 0;
        enemies = biome.enemyPool;
        enemyNumbers = biome.enemyCount;
    }

    public void GenerateEnemies(Room room, BiomeData biome)
    {
        if (biome.enemyPool.Count == 0)
            return;

        Setup(biome);

        int enemyNumber = SelectWeightedPool();

        List<Vector3> spawnPoints = TilemapScraper.FindSpawnPoints(room, spawnMarker, "Markers/Enemy Marker");
        ShuffleList(spawnPoints);
        int enemiesToSpawn = Mathf.Min(enemyNumber, spawnPoints.Count);

        for (int e = 0; e < enemiesToSpawn; e++)
        {
            SpawnEnemy(spawnPoints[e], room);
        }

        ladder.AddEnemies(totalEnemies);
    }

    private void SpawnEnemy(Vector3 spot, Room room)
    {
        GameObject enemy = enemies[Random.Range(0, enemies.Count)];
        GameObject spawnedEnemy = Instantiate(enemy, spot, Quaternion.identity, room.transform);

        EnemyBase enemyBase = spawnedEnemy.GetComponent<EnemyBase>();
        enemyBase.InitializeEnemy(room.RoomID);

        totalEnemies++;
    }

    private int SelectWeightedPool()
    {
        float totalWeight = 0f;
        foreach (var number in enemyNumbers)
            totalWeight += number.spawnChance;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var number in enemyNumbers)
        {
            cumulative += number.spawnChance;
            if (roll <= cumulative)
                return number.enemyNumber;
        }

        // Fallback in case of floating point rounding at the boundary
        return enemyNumbers[enemyNumbers.Count - 1].enemyNumber;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}