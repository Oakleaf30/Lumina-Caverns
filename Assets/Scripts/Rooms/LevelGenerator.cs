using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room[] roomPrefabs;
    public int maxRooms = 12;
    public float roomSize = 50f;

    [SerializeField] private InteractiveGenerator itemGenerator;

    // This tracks exactly what doors each grid coordinate NEEDS
    private Dictionary<Vector2Int, RoomRequirements> layout = new Dictionary<Vector2Int, RoomRequirements>();
    private Dictionary<Vector2Int, Room> spawnedRooms = new Dictionary<Vector2Int, Room>();

    private class RoomRequirements
    {
        public bool top, bottom, left, right;

        public int RoomID;

        public RoomRequirements(int id)
        {
            RoomID = id;
        }
    }

    void Start() => Generate();

    void Generate()
    {
        Vector2Int startPos = Vector2Int.zero;

        // Start our counter at 0. This will serve as both our ID and our maxRooms limiter.
        int roomsPlaced = 0;

        // Assign ID 0 to the starting room
        layout.Add(startPos, new RoomRequirements(roomsPlaced));
        Queue<Vector2Int> checkQueue = new Queue<Vector2Int>();
        checkQueue.Enqueue(startPos);

        // Increment immediately so the next room placed gets ID 1
        roomsPlaced++;

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
                        // Pass the current counter value as the unique ID for this new room
                        layout.Add(neighborPos, new RoomRequirements(roomsPlaced));
                        Connect(currentPos, neighborPos, dir);
                        checkQueue.Enqueue(neighborPos);

                        // Increment the counter/ID for the next iteration
                        roomsPlaced++;
                    }
                }
            }
            // If we run out of steam, re-add a random room to keep growing
            if (checkQueue.Count == 0 && roomsPlaced < maxRooms)
                checkQueue.Enqueue(new List<Vector2Int>(layout.Keys)[Random.Range(0, layout.Count)]);
        }

        SpawnRooms();

        LinkAllDoors();
    }

    void LinkAllDoors()
    {
        foreach (var kvp in spawnedRooms)
        {
            Vector2Int pos = kvp.Key;
            Room current = kvp.Value;

            // Link North neighbor
            if (spawnedRooms.TryGetValue(pos + Vector2Int.up, out Room northNeighbor))
            {
                if (current.topDoor != null && northNeighbor.bottomDoor != null)
                    current.topDoor.connectedDoor = northNeighbor.bottomDoor;
            }

            // Link South neighbor
            if (spawnedRooms.TryGetValue(pos + Vector2Int.down, out Room southNeighbor))
            {
                if (current.bottomDoor != null && southNeighbor.topDoor != null)
                    current.bottomDoor.connectedDoor = southNeighbor.topDoor;
            }

            // Link East neighbor
            if (spawnedRooms.TryGetValue(pos + Vector2Int.right, out Room eastNeighbor))
            {
                if (current.rightDoor != null && eastNeighbor.leftDoor != null)
                    current.rightDoor.connectedDoor = eastNeighbor.leftDoor;
            }

            // Link West neighbor
            if (spawnedRooms.TryGetValue(pos + Vector2Int.left, out Room westNeighbor))
            {
                if (current.leftDoor != null && westNeighbor.rightDoor != null)
                    current.leftDoor.connectedDoor = westNeighbor.rightDoor;
            }
        }
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

        itemGenerator.GenerateLevelInteractivity();
    }

    //void PlaceBestRoom(Vector2Int pos, RoomRequirements req)
    //{
    //    foreach (Room prefab in roomPrefabs)
    //    {
    //        for (int rot = 0; rot < 4; rot++)
    //        {
    //            // MATCHING LOGIC: Prefab must have DOORS where layout says YES
    //            // and NO DOORS where layout says NO.
    //            if (prefab.HasDoor("Top", rot) == req.top &&
    //                prefab.HasDoor("Bottom", rot) == req.bottom &&
    //                prefab.HasDoor("Left", rot) == req.left &&
    //                prefab.HasDoor("Right", rot) == req.right)
    //            {
    //                Vector3 worldPos = new Vector3(pos.x * roomSize, pos.y * roomSize, 0);
    //                Room spawned = Instantiate(prefab, worldPos, Quaternion.Euler(0, 0, -90 * rot));
    //                return;
    //            }
    //        }
    //    }
    //}

    void PlaceBestRoom(Vector2Int pos, RoomRequirements req)
    {
        // 1. Shuffle the prefabs so we get visual variety if multiple rooms have the same door layout
        List<Room> shuffledPrefabs = new List<Room>(roomPrefabs);
        for (int i = 0; i < shuffledPrefabs.Count; i++)
        {
            Room temp = shuffledPrefabs[i];
            int r = Random.Range(i, shuffledPrefabs.Count);
            shuffledPrefabs[i] = shuffledPrefabs[r];
            shuffledPrefabs[r] = temp;
        }

        // 2. Find the perfect match
        foreach (Room prefab in shuffledPrefabs)
        {
            if (prefab.hasTopDoor == req.top &&
                prefab.hasBottomDoor == req.bottom &&
                prefab.hasLeftDoor == req.left &&
                prefab.hasRightDoor == req.right)
            {
                Vector3 worldPos = new Vector3(pos.x * roomSize, pos.y * roomSize, 0);
                Room newRoom = Instantiate(prefab, worldPos, Quaternion.identity);

                // --- NEW: Stamp the physical room with the ID from your data graph! ---
                newRoom.InitializeRoom(req.RoomID);

                // Save the reference to the room we just made
                spawnedRooms.Add(pos, newRoom);
                return;
            }
        }

        // 3. Fallback warning
        // If you forget to make one of the 15 possible door combinations, this will tell you exactly which one is missing!
        Debug.LogWarning($"No prefab found for room at {pos}! Needs -> Top:{req.top} Bottom:{req.bottom} Left:{req.left} Right:{req.right}");
    }
}