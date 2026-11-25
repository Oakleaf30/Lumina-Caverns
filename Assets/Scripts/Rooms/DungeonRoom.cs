using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class DungeonRoom : Room
{
    [Header("Dungeon References")]
    [SerializeField] private GameEvent onDungeonCleared;
    [SerializeField] private GameEvent OnDungeonEntered;
    [SerializeField] private GameObject entrance;

    

    private void OnEnable()
    {
        if (onDungeonCleared != null)
        {
            onDungeonCleared.Subscribe(OpenDoor);
        }

        var map = transform.Find("Props/Spikes")?.GetComponent<Tilemap>();
        SpikeManager.Instance.SetMap(map);   // null if this room has no spikes
    }

    private void OnDisable()
    {
        if (onDungeonCleared != null)
        {
            onDungeonCleared.Unsubscribe(OpenDoor);
        }

        if (SpikeManager.Instance != null)
            SpikeManager.Instance.SetMap(null);
    }

    public void BlockDungeonDoors()
    {
        wallTopMap.AddComponent<TilemapCollider2D>();
    }

    private void OpenDoor()
    {
        wallTopMap.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnDungeonEntered.Raise();

            if (entrance != null)
                entrance.SetActive(true);
        }
            
    }
}
