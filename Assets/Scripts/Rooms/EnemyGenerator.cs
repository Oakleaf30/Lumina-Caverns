using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private TileBase spawnMarker;
    [SerializeField] private GameObject enemy;

    public void GenerateEnemies(Room room)
    {
        List<Vector3> spawnPoints = TilemapScraper.FindSpawnPoints(room, spawnMarker, "Markers/Enemy Marker");

        SpawnEnemy(spawnPoints, room);
    }

    private void SpawnEnemy(List<Vector3> points, Room room) // Pass the whole Room
    {
        foreach (Vector3 spot in points)
        {
            if (enemy != null)
            {
                // 1. Instantiate and capture the reference
                GameObject spawnedEnemy = Instantiate(enemy, spot, Quaternion.identity, room.transform);

                // 2. Inject the data
                EnemyBase enemyBase = spawnedEnemy.GetComponent<EnemyBase>();
                if (enemyBase != null)
                {
                    enemyBase.InitializeEnemy(room.RoomID); // Assuming Room has a public int RoomID
                }
            }
        }
    }
}