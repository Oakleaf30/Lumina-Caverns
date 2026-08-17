using UnityEngine;
using UnityEngine.Tilemaps;

public class Cauldron : MonoBehaviour
{
    private RunState RunState => GameSession.Instance.runState;

    private int FloorCounter
    {
        get => GameSession.Instance.runState.cauldronFloorTracker;
        set => GameSession.Instance.runState.cauldronFloorTracker = value;
    }

    [SerializeField] private GameEvent onLadderUsed;
    [SerializeField] private GameEvent onShaftUsed;
    [SerializeField] private TileBase marker;
    [SerializeField] private GameObject cauldron;

    [SerializeField] private int minFloor = 5;
    [SerializeField] private float baseChance = 0.5f;
    [SerializeField] private float chancePerFloor = 0.1f;

    private bool cauldronSpawnable = false;
    private int cauldronRoom;

    private void OnEnable()
    {
        onLadderUsed.Subscribe(NextFloor);
        onShaftUsed.Subscribe(NextFloor);
    }

    private void OnDisable()
    {
        onLadderUsed.Unsubscribe(NextFloor);
        onShaftUsed.Unsubscribe(NextFloor);
    }

    private void NextFloor()
    {
        FloorCounter++;
        
    }

    private float GetCauldronChance()
    {
        if (FloorCounter < minFloor)
            return 0f;

        float chance = baseChance + chancePerFloor * (FloorCounter - minFloor);
        return Mathf.Clamp01(chance);
    }

    public bool RoomCheck(int roomCounter, int roomAmount)
    {
        if (roomCounter == 0)
        {
            cauldronRoom = Random.Range(1, roomAmount);
            if (Random.value < GetCauldronChance())
                cauldronSpawnable = true;
        }

        return roomCounter == cauldronRoom && cauldronSpawnable;
    }

    public void SpawnCauldron(Room room)
    {
        Vector3 spawnPoint = TilemapScraper.FindSpawnPoint(room, marker, "Markers/Cauldron Marker");
        Instantiate(cauldron, spawnPoint, Quaternion.identity);
        cauldronSpawnable = false;
        FloorCounter = 0;
    }
}
