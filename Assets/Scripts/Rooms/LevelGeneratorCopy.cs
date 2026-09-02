//using System.Collections.Generic;
//using UnityEngine;

//public class LevelGenerator : MonoBehaviour
//{
//    [SerializeField] private Room[] roomPrefabs;
//    [SerializeField] private int maxRooms;
//    [SerializeField] private float roomSize;
//    public BiomeData biomeData;

//    [SerializeField] private InteractiveGenerator itemGenerator;
//    [SerializeField] private EnemyGenerator enemyGenerator;
//    [SerializeField] private Cauldron cauldron;
//    private bool cauldronSpawnable = false;

//    private Dictionary<Vector2Int, RoomRequirements> layout = new Dictionary<Vector2Int, RoomRequirements>();
//    private Dictionary<Vector2Int, Room> spawnedRooms = new Dictionary<Vector2Int, Room>();

//    private class RoomRequirements
//    {
//        public bool top, bottom, left, right;
//        public int RoomID;

//        public RoomRequirements(int id)
//        {
//            RoomID = id;
//        }
//    }

//    void Awake() => Generate();

//    void Generate()
//    {
//        int safetyLimit = 500; // Prevents an infinite loop if a valid configuration is mathematically impossible
//        int attempts = 0;
//        bool validLayoutFound = false;

//        if (TransitionState.ConsumePendingTransition(out var biome)) biomeData = biome;

//        while (!validLayoutFound && attempts < safetyLimit)
//        {
//            attempts++;
//            layout.Clear();
//            spawnedRooms.Clear();

//            Vector2Int startPos = Vector2Int.zero;
//            int roomsPlaced = 0;

//            // Assign ID 0 to the starting room
//            layout.Add(startPos, new RoomRequirements(roomsPlaced));
//            Queue<Vector2Int> checkQueue = new Queue<Vector2Int>();
//            checkQueue.Enqueue(startPos);

//            roomsPlaced++;

//            while (checkQueue.Count > 0 && roomsPlaced < maxRooms)
//            {
//                Vector2Int currentPos = checkQueue.Dequeue();

//                foreach (Vector2Int dir in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
//                {
//                    Vector2Int neighborPos = currentPos + dir;

//                    if (roomsPlaced < maxRooms && !layout.ContainsKey(neighborPos) && Random.value < 0.5f)
//                    {
//                        if (CountNeighbors(neighborPos) == 1)
//                        {
//                            layout.Add(neighborPos, new RoomRequirements(roomsPlaced));
//                            Connect(currentPos, neighborPos, dir);
//                            checkQueue.Enqueue(neighborPos);
//                            roomsPlaced++;
//                        }
//                    }
//                }

//                if (checkQueue.Count == 0 && roomsPlaced < maxRooms)
//                    checkQueue.Enqueue(new List<Vector2Int>(layout.Keys)[Random.Range(0, layout.Count)]);
//            }

//            // Verify if every single data requirement we just mapped out has a matching physical asset
//            if (ValidateLayout())
//            {
//                validLayoutFound = true;
//            }
//        }

//        if (!validLayoutFound)
//        {
//            Debug.LogError($"Level Generator gave up after {safetyLimit} attempts! Ensure you haven't completely bottlenecked your layout options.");
//            return;
//        }

//        // We have a 100% verified, perfectly matchable blueprint. Now build it physically!
//        SpawnRooms();
//        LinkAllDoors();
//    }

//    // New helper method that double-checks the your scriptable/prefab asset compatibility
//    bool ValidateLayout()
//    {
//        foreach (var req in layout.Values)
//        {
//            bool matchFound = false;
//            foreach (Room prefab in roomPrefabs)
//            {
//                if (prefab.hasTopDoor == req.top &&
//                    prefab.hasBottomDoor == req.bottom &&
//                    prefab.hasLeftDoor == req.left &&
//                    prefab.hasRightDoor == req.right)
//                {
//                    matchFound = true;
//                    break;
//                }
//            }

//            // If even ONE room requirement layout cannot be satisfied, fail the validation immediately
//            if (!matchFound)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    void LinkAllDoors()
//    {
//        foreach (var kvp in spawnedRooms)
//        {
//            Vector2Int pos = kvp.Key;
//            Room current = kvp.Value;

//            if (spawnedRooms.TryGetValue(pos + Vector2Int.up, out Room northNeighbor))
//            {
//                if (current.topDoor != null && northNeighbor.bottomDoor != null)
//                    current.topDoor.connectedDoor = northNeighbor.bottomDoor;
//            }

//            if (spawnedRooms.TryGetValue(pos + Vector2Int.down, out Room southNeighbor))
//            {
//                if (current.bottomDoor != null && southNeighbor.topDoor != null)
//                    current.bottomDoor.connectedDoor = southNeighbor.topDoor;
//            }

//            if (spawnedRooms.TryGetValue(pos + Vector2Int.right, out Room eastNeighbor))
//            {
//                if (current.rightDoor != null && eastNeighbor.leftDoor != null)
//                    current.rightDoor.connectedDoor = eastNeighbor.leftDoor;
//            }

//            if (spawnedRooms.TryGetValue(pos + Vector2Int.left, out Room westNeighbor))
//            {
//                if (current.leftDoor != null && westNeighbor.rightDoor != null)
//                    current.leftDoor.connectedDoor = westNeighbor.rightDoor;
//            }
//        }
//    }

//    void Connect(Vector2Int a, Vector2Int b, Vector2Int dir)
//    {
//        if (dir == Vector2Int.up) { layout[a].top = true; layout[b].bottom = true; }
//        if (dir == Vector2Int.down) { layout[a].bottom = true; layout[b].top = true; }
//        if (dir == Vector2Int.right) { layout[a].right = true; layout[b].left = true; }
//        if (dir == Vector2Int.left) { layout[a].left = true; layout[b].right = true; }
//    }

//    int CountNeighbors(Vector2Int pos)
//    {
//        int count = 0;
//        foreach (Vector2Int d in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
//            if (layout.ContainsKey(pos + d)) count++;
//        return count;
//    }

//    void SpawnRooms()
//    {
//        int roomCounter = 0;

//        foreach (var kvp in layout)
//        {
//            cauldronSpawnable = cauldron.RoomCheck(roomCounter, layout.Count);
//            PlaceBestRoom(kvp.Key, kvp.Value);
//            roomCounter++;
//        }
//    }

//    void PlaceBestRoom(Vector2Int pos, RoomRequirements req)
//    {
//        List<Room> shuffledPrefabs = new List<Room>(roomPrefabs);
//        for (int i = 0; i < shuffledPrefabs.Count; i++)
//        {
//            Room temp = shuffledPrefabs[i];
//            int r = Random.Range(i, shuffledPrefabs.Count);
//            shuffledPrefabs[i] = shuffledPrefabs[r];
//            shuffledPrefabs[r] = temp;
//        }

//        foreach (Room prefab in shuffledPrefabs)
//        {
//            if (prefab.hasTopDoor == req.top &&
//                prefab.hasBottomDoor == req.bottom &&
//                prefab.hasLeftDoor == req.left &&
//                prefab.hasRightDoor == req.right)
//            {
//                Vector3 worldPos = new Vector3(pos.x * roomSize, pos.y * roomSize, 0);
//                Room newRoom = Instantiate(prefab, worldPos, Quaternion.identity, transform);
//                newRoom.InitializeRoom(req.RoomID);

//                itemGenerator.GenerateInteractivity(newRoom, biomeData);
//                enemyGenerator.GenerateEnemies(newRoom, biomeData);

//                if (cauldronSpawnable)
//                    cauldron.SpawnCauldron(newRoom);

//                cauldronSpawnable = false;

//                spawnedRooms.Add(pos, newRoom);
//                return;
//            }
//        }

//        Debug.LogWarning($"No prefab found for room at {pos}! Needs -> Top:{req.top} Bottom:{req.bottom} Left:{req.left} Right:{req.right}");
//    }
//}