using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveGenerator : MonoBehaviour
{
    [SerializeField] private TileBase interactiveMarker;
    [SerializeField] private GameObject OrePrefab;

    public void GenerateInteractivity(Room room)
    {
        List<Vector3> spawnPoints = TilemapScraper.FindSpawnPoints(room, interactiveMarker, "Markers/Interaction Marker");

        SpawnOres(spawnPoints, room.transform);
    }

    private void SpawnOres(List<Vector3> points, Transform parent)
    {
        foreach (Vector3 spot in points)
        {
            if (OrePrefab != null)
            {
                Instantiate(OrePrefab, spot, Quaternion.identity, parent);
            }
        }
    }
}