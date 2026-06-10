using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveGenerator : MonoBehaviour
{
    [SerializeField] private TileBase genericSpawnMarker;
    [SerializeField] private GameObject placeholderOrePrefab;

    public void GenerateLevelInteractivity()
    {
        // Check if inspector fields are missing before running logic
        if (genericSpawnMarker == null || placeholderOrePrefab == null)
        {
            return;
        }

        // Find the rooms active in the scene
        Room[] activeRooms = FindObjectsByType<Room>(FindObjectsSortMode.None);

        foreach (Room room in activeRooms)
        {
            // 1. Find the specific child GameObject by its exact name string
            // Use a path string if your marker is inside a child container
            Transform markerTransform = room.transform.Find("Markers/Interaction Marker");

            if (markerTransform != null)
            {
                // 2. Grab the Tilemap component directly off that targeted object
                Tilemap markerMap = markerTransform.GetComponent<Tilemap>();

                if (markerMap != null)
                {
                    List<Vector3> spawnPoints = ScrapePoints(markerMap);

                    if (spawnPoints.Count > 0)
                    {
                        SpawnTestOres(spawnPoints, room.transform);
                    }
                }
            }
        }
    }

    private List<Vector3> ScrapePoints(Tilemap map)
    {
        List<Vector3> points = new List<Vector3>();
        map.CompressBounds();

        BoundsInt bounds = map.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (map.HasTile(pos))
            {
                TileBase currentTile = map.GetTile(pos);

                // Compare the tiles
                if (currentTile == genericSpawnMarker)
                {
                    Vector3 worldPos = map.GetCellCenterWorld(pos);
                    points.Add(worldPos);
                }
            }
        }

        return points;
    }

    private void SpawnTestOres(List<Vector3> points, Transform parent)
    {
        foreach (Vector3 spot in points)
        {
            if (placeholderOrePrefab != null)
            {
                Instantiate(placeholderOrePrefab, spot, Quaternion.identity, parent);
            }
        }
    }
}