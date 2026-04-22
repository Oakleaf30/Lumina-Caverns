using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room[] roomPrefabs;
    public int maxRooms = 12;
    public float roomSize = 50f;

    // This tracks exactly what doors each grid coordinate NEEDS
    private Dictionary<Vector2Int, RoomRequirements> layout = new Dictionary<Vector2Int, RoomRequirements>();

    private class RoomRequirements
    {
        public bool top, bottom, left, right;
    }

    void Start() => Generate();

    void Generate()
    {
        Vector2Int startPos = Vector2Int.zero;
        layout.Add(startPos, new RoomRequirements());
        Queue<Vector2Int> checkQueue = new Queue<Vector2Int>();
        checkQueue.Enqueue(startPos);

        int roomsPlaced = 1;

        while (checkQueue.Count > 0 && roomsPlaced < maxRooms)
        {
            Vector2Int currentPos = checkQueue.Dequeue();

            foreach (Vector2Int dir in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentPos + dir;

                if (roomsPlaced < maxRooms && !layout.ContainsKey(neighborPos) && Random.value < 0.5f)
                {
                    // Isaac Rule: Only place if it has exactly 1 neighbor (prevents clumping)
                    if (CountNeighbors(neighborPos) == 1)
                    {
                        layout.Add(neighborPos, new RoomRequirements());
                        Connect(currentPos, neighborPos, dir);
                        checkQueue.Enqueue(neighborPos);
                        roomsPlaced++;
                    }
                }
            }
            // If we run out of steam, re-add a random room to keep growing
            if (checkQueue.Count == 0 && roomsPlaced < maxRooms)
                checkQueue.Enqueue(new List<Vector2Int>(layout.Keys)[Random.Range(0, layout.Count)]);
        }

        SpawnRooms();
    }

    void Connect(Vector2Int a, Vector2Int b, Vector2Int dir)
    {
        if (dir == Vector2Int.up) { layout[a].top = true; layout[b].bottom = true; }
        if (dir == Vector2Int.down) { layout[a].bottom = true; layout[b].top = true; }
        if (dir == Vector2Int.right) { layout[a].right = true; layout[b].left = true; }
        if (dir == Vector2Int.left) { layout[a].left = true; layout[b].right = true; }
    }

    int CountNeighbors(Vector2Int pos)
    {
        int count = 0;
        foreach (Vector2Int d in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            if (layout.ContainsKey(pos + d)) count++;
        return count;
    }

    void SpawnRooms()
    {
        foreach (var kvp in layout)
        {
            PlaceBestRoom(kvp.Key, kvp.Value);
        }
    }

    void PlaceBestRoom(Vector2Int pos, RoomRequirements req)
    {
        foreach (Room prefab in roomPrefabs)
        {
            for (int rot = 0; rot < 4; rot++)
            {
                // MATCHING LOGIC: Prefab must have DOORS where layout says YES
                // and NO DOORS where layout says NO.
                if (prefab.HasDoor("Top", rot) == req.top &&
                    prefab.HasDoor("Bottom", rot) == req.bottom &&
                    prefab.HasDoor("Left", rot) == req.left &&
                    prefab.HasDoor("Right", rot) == req.right)
                {
                    Vector3 worldPos = new Vector3(pos.x * roomSize, pos.y * roomSize, 0);
                    Room spawned = Instantiate(prefab, worldPos, Quaternion.Euler(0, 0, -90 * rot));
                    return;
                }
            }
        }
    }

    //void PlaceBestRoom(Vector2Int pos, RoomRequirements req)
    //{
    //    // 1. Shuffle the prefabs so we get visual variety if multiple rooms have the same door layout
    //    List<Room> shuffledPrefabs = new List<Room>(roomPrefabs);
    //    for (int i = 0; i < shuffledPrefabs.Count; i++)
    //    {
    //        Room temp = shuffledPrefabs[i];
    //        int r = Random.Range(i, shuffledPrefabs.Count);
    //        shuffledPrefabs[i] = shuffledPrefabs[r];
    //        shuffledPrefabs[r] = temp;
    //    }

    //    // 2. Find the perfect match
    //    foreach (Room prefab in shuffledPrefabs)
    //    {
    //        // MATCHING LOGIC: Check the default door booleans directly, NO rotation math
    //        if (prefab.hasTopDoor == req.top &&
    //            prefab.hasBottomDoor == req.bottom &&
    //            prefab.hasLeftDoor == req.left &&
    //            prefab.hasRightDoor == req.right)
    //        {
    //            Vector3 worldPos = new Vector3(pos.x * roomSize, pos.y * roomSize, 0);

    //            // Instantiate with default rotation (Quaternion.identity)
    //            Instantiate(prefab, worldPos, Quaternion.identity);
    //            return; // We found our room, exit the function
    //        }
    //    }

    //    // 3. Fallback warning
    //    // If you forget to make one of the 15 possible door combinations, this will tell you exactly which one is missing!
    //    Debug.LogWarning($"No prefab found for room at {pos}! Needs -> Top:{req.top} Bottom:{req.bottom} Left:{req.left} Right:{req.right}");
    //}
}