using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Settings")]
    public Room[] roomPrefabs; // Drag all your room variants here
    public int minRooms = 4;
    public int maxRooms = 8;
    public int roomSize = 20; // 20x20

    // Internal grid to track where we decided to put rooms
    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
    private List<Vector2Int> pathOrder = new List<Vector2Int>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // 1. Generate the Abstract Path (The "Snake")
        Vector2Int currentPos = Vector2Int.zero;
        pathOrder.Add(currentPos);
        occupiedPositions.Add(currentPos);

        int targetCount = Random.Range(minRooms, maxRooms);

        for (int i = 0; i < targetCount - 1; i++)
        {
            // Pick a random neighbor that isn't occupied
            Vector2Int nextPos = GetValidNeighbor(currentPos);

            // If we get stuck (boxed in), stop generation or restart
            if (nextPos == currentPos) break;

            pathOrder.Add(nextPos);
            occupiedPositions.Add(nextPos);
            currentPos = nextPos;
        }

        // 2. Place the actual Rooms based on connections
        foreach (Vector2Int pos in pathOrder)
        {
            PlaceBestRoom(pos);
        }
    }

    Vector2Int GetValidNeighbor(Vector2Int current)
    {
        // Try all 4 directions randomly
        List<Vector2Int> neighbors = new List<Vector2Int> {
            current + Vector2Int.up,
            current + Vector2Int.down,
            current + Vector2Int.left,
            current + Vector2Int.right
        };

        // Shuffle list for randomness
        for (int i = 0; i < neighbors.Count; i++)
        {
            Vector2Int temp = neighbors[i];
            int r = Random.Range(i, neighbors.Count);
            neighbors[i] = neighbors[r];
            neighbors[r] = temp;
        }

        // Return the first one that hasn't been visited
        foreach (var n in neighbors)
        {
            if (!occupiedPositions.Contains(n)) return n;
        }

        return current; // Return self if stuck
    }

    void PlaceBestRoom(Vector2Int gridPos)
    {
        // Determine required doors based on neighbors in the path
        bool needTop = occupiedPositions.Contains(gridPos + Vector2Int.up);
        bool needBottom = occupiedPositions.Contains(gridPos + Vector2Int.down);
        bool needLeft = occupiedPositions.Contains(gridPos + Vector2Int.left);
        bool needRight = occupiedPositions.Contains(gridPos + Vector2Int.right);

        // Find a prefab + rotation that matches needs
        // We shuffle the prefabs so we don't always pick the first one in the list
        List<Room> shuffledPrefabs = new List<Room>(roomPrefabs);

        // Simple shuffle
        for (int i = 0; i < shuffledPrefabs.Count; i++)
        {
            Room temp = shuffledPrefabs[i];
            int r = Random.Range(i, shuffledPrefabs.Count);
            shuffledPrefabs[i] = shuffledPrefabs[r];
            shuffledPrefabs[r] = temp;
        }

        foreach (Room prefab in shuffledPrefabs)
        {
            // Try all 4 rotations (0, 90, 180, 270)
            // 0=0deg, 1=-90deg, 2=-180deg, 3=-270deg
            for (int rot = 0; rot < 4; rot++)
            {
                // Strict check: Does this rotation have ALL the doors we need?
                // (You can add logic here to allow EXTRA doors if you want branching)
                bool valid = true;

                if (needTop && !prefab.HasDoor("Top", rot)) valid = false;
                if (needBottom && !prefab.HasDoor("Bottom", rot)) valid = false;
                if (needLeft && !prefab.HasDoor("Left", rot)) valid = false;
                if (needRight && !prefab.HasDoor("Right", rot)) valid = false;

                if (valid)
                {
                    // Found a match! Instantiate it.
                    Vector3 worldPos = new Vector3(gridPos.x * roomSize, gridPos.y * roomSize, 0);
                    Room newRoom = Instantiate(prefab, worldPos, Quaternion.identity);

                    // Apply rotation (negative because Unity rotates Counter-Clockwise, but our logic stepped Clockwise)
                    newRoom.transform.rotation = Quaternion.Euler(0, 0, -90 * rot);
                    return; // Done with this room
                }
            }
        }

        Debug.LogWarning("No room fit the requirements for " + gridPos);
    }
}