using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapScraper
{
    public static List<Vector3> FindSpawnPoints(Room room, TileBase marker, string markerLayer)
    {
        Transform markerTransform = room.transform.Find(markerLayer);

        if (markerTransform != null)
        {
            // 2. Grab the Tilemap component directly off that targeted object
            Tilemap markerMap = markerTransform.GetComponent<Tilemap>();

            if (markerMap != null)
            {
                return ScrapePoints(markerMap, marker);
            }
        }

        return new List<Vector3>();
    }

    private static List<Vector3> ScrapePoints(Tilemap map, TileBase marker)
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
                if (currentTile == marker)
                {
                    Vector3 worldPos = map.GetCellCenterWorld(pos);
                    points.Add(worldPos);
                }
            }
        }

        return points;
    }

    public static Vector3 FindSpawnPoint(Room room, TileBase marker, string markerLayer)
    {
        Tilemap map = room.transform.Find(markerLayer)?.GetComponent<Tilemap>();

        if (map == null)
            return Vector3.zero;

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (map.HasTile(pos) && map.GetTile(pos) == marker)
                return map.GetCellCenterWorld(pos);
        }

        return Vector3.zero;
    }
}
