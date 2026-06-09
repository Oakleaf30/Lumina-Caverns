using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveGenerator : MonoBehaviour
{
    [SerializeField] private TileBase genericSpawnMarker;
    [SerializeField] private GameObject placeholderOrePrefab;

    public void GenerateLevelInteractivity()
    {
        Debug.Log("<color=cyan>[InteractiveGenerator]</color> Start() initializing level spawning setup.");

        // Check if inspector fields are missing before running logic
        if (genericSpawnMarker == null)
        {
            Debug.LogError("<color=red>[InteractiveGenerator]</color> CRITICAL: 'genericSpawnMarker' is not assigned in the Inspector!");
        }
        if (placeholderOrePrefab == null)
        {
            Debug.LogError("<color=red>[InteractiveGenerator]</color> CRITICAL: 'placeholderOrePrefab' is not assigned in the Inspector!");
        }

        // For testing, just find the rooms active in your test scene
        Room[] activeRooms = FindObjectsByType<Room>(FindObjectsSortMode.None);

        Debug.Log($"<color=cyan>[InteractiveGenerator]</color> Found {activeRooms.Length} active Room component(s) in the scene.");

        if (activeRooms.Length == 0)
        {
            Debug.LogWarning("<color=yellow>[InteractiveGenerator]</color> No Room components found in the scene! Ensure your room instances have the 'Room' script attached.");
        }

        foreach (Room room in activeRooms)
        {
            Debug.Log($"<color=white> -> Processing Room: {room.name}</color>");

            // 1. Find the specific child GameObject by its exact name string
            // Use a path string if your marker is inside a child container
            Transform markerTransform = room.transform.Find("Markers/Interaction Marker");

            if (markerTransform != null)
            {
                Debug.Log($"   <color=green>SUCCESS:</color> Found child GameObject 'Interaction Marker' inside {room.name}.");

                // 2. Grab the Tilemap component directly off that targeted object
                Tilemap markerMap = markerTransform.GetComponent<Tilemap>();

                if (markerMap != null)
                {
                    Debug.Log($"   <color=green>SUCCESS:</color> Retrieved Tilemap component from 'Interaction Marker' in {room.name}.");

                    List<Vector3> spawnPoints = ScrapePoints(markerMap);

                    Debug.Log($"   <color=cyan>RESULT:</color> Scraped {spawnPoints.Count} matching coordinates from {room.name}'s marker map.");

                    if (spawnPoints.Count > 0)
                    {
                        SpawnTestOres(spawnPoints, room.transform);
                    }
                }
                else
                {
                    Debug.LogError($"   <color=red>ERROR:</color> The GameObject 'Interaction Marker' in {room.name} does not have a Tilemap component attached!");
                }
            }
            else
            {
                // Notice: Fixed a typo from your previous Warning message to match your target name "Interaction Marker"
                Debug.LogWarning($"   <color=yellow>WARNING:</color> Room {room.name} is missing an immediate child object named exactly 'Interaction Marker'!");
            }
        }
    }

    private List<Vector3> ScrapePoints(Tilemap map)
    {
        List<Vector3> points = new List<Vector3>();
        map.CompressBounds();

        BoundsInt bounds = map.cellBounds;
        int totalTilesInspected = 0;
        int matchingTilesFound = 0;

        foreach (var pos in bounds.allPositionsWithin)
        {
            totalTilesInspected++;

            if (map.HasTile(pos))
            {
                TileBase currentTile = map.GetTile(pos);

                // Compare the tiles
                if (currentTile == genericSpawnMarker)
                {
                    matchingTilesFound++;
                    Vector3 worldPos = map.GetCellCenterWorld(pos);
                    points.Add(worldPos);
                }
            }
        }

        Debug.Log($"     [ScrapePoints] Grid scanning complete for {map.gameObject.name}. Total coordinates checked: {totalTilesInspected}. Valid occupied tiles found matching the marker asset reference: {matchingTilesFound}.");
        return points;
    }

    private void SpawnTestOres(List<Vector3> points, Transform parent)
    {
        int spawnCount = 0;
        foreach (Vector3 spot in points)
        {
            if (placeholderOrePrefab != null)
            {
                Instantiate(placeholderOrePrefab, spot, Quaternion.identity, parent);
                spawnCount++;
            }
        }
        Debug.Log($"     <color=green>[SpawnTestOres]</color> Successfully instantiated {spawnCount} objects as children under {parent.name}.");
    }
}