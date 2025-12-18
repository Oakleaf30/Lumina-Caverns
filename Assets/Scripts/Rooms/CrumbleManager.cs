using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CrumbleManager : MonoBehaviour
{
    [Header("Settings")]
    public float respawnTime = 5f;

    [Header("Assets (Drag into Inspector)")]
    public TileBase tileIntact;     // The normal floor tile
    public TileBase markerCrumble;  // The marker you paint in the editor
    public GameObject crumbleAnimPrefab; // The animation effect prefab

    // Private References (Found in Awake)
    private Tilemap tilemapFloor;
    private Tilemap tilemapMarkers;

    // This tracks which tiles are currently broken and when they broke.
    private Dictionary<Vector3Int, float> brokenTiles = new Dictionary<Vector3Int, float>();

    // This tracks ALL positions that are designated to be crumble tiles (used for quick lookup).
    private HashSet<Vector3Int> validCrumblePositions = new HashSet<Vector3Int>();

    void Awake()
    {
        Tilemap[] maps = GetComponentsInChildren<Tilemap>();

        foreach (var t in maps)
        {
            if (t.name == "Breakable")
                tilemapFloor = t;
            else if (t.name == "Breakable Marker")
                tilemapMarkers = t;
        }

        if (tilemapFloor == null || tilemapMarkers == null)
        {
            Debug.LogError("CrumbleTileManager could not find one or both necessary Tilemaps (Tilemap_Floor / Tilemap_Markers) as children of the Room.");
            enabled = false;
            return;
        }

        InitializeRoom();
    }

    public void InitializeRoom()
    {
        // We only scan the area where tiles exist on the marker map.
        foreach (var pos in tilemapMarkers.cellBounds.allPositionsWithin)
        {
            if (tilemapMarkers.GetTile(pos) == markerCrumble)
            {
                // 1. Add position to the set for quick lookup during gameplay
                validCrumblePositions.Add(pos);

                // 2. Set the initial visual tile on the FLOOR layer
                tilemapFloor.SetTile(pos, tileIntact);

                // 3. IMPORTANT: Remove the marker tile
                tilemapMarkers.SetTile(pos, null);
            }
        }
    }

    public void ProcessPlayerStep(Vector3Int cellPos)
    {
        // 1. Check if it's a designated crumble spot (using the new HashSet)
        // AND not already broken/on the respawn timer
        if (validCrumblePositions.Contains(cellPos) && !brokenTiles.ContainsKey(cellPos))
        {
            // 2. IMMEDIATE ACTION: Break the tile!
            BreakTile(cellPos);
        }
    }

    // ... (The rest of the BreakTile and Update functions remain the same) ...

    void BreakTile(Vector3Int pos)
    {
        tilemapFloor.SetTile(pos, null);
        Vector3 worldPos = tilemapFloor.GetCellCenterWorld(pos);
        Instantiate(crumbleAnimPrefab, worldPos, Quaternion.identity);
        brokenTiles.Add(pos, Time.time);
    }

    void Update()
    {
        List<Vector3Int> tilesToRespawn = new List<Vector3Int>();

        foreach (var kvp in brokenTiles)
        {
            if (Time.time >= kvp.Value + respawnTime)
            {
                tilesToRespawn.Add(kvp.Key);
            }
        }

        foreach (var pos in tilesToRespawn)
        {
            tilemapFloor.SetTile(pos, tileIntact);
            brokenTiles.Remove(pos);
        }
    }
}