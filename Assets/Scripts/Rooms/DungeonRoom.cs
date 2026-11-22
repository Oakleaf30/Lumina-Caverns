using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class DungeonRoom : Room
{
    [Header("Dungeon References")]
    [SerializeField] private GameEvent onDungeonCleared;
    [SerializeField] private GameObject entrance;

    

    private void OnEnable()
    {
        if (onDungeonCleared != null)
        {
            onDungeonCleared.Subscribe(OpenDoor);
        }
    }

    private void OnDisable()
    {
        if (onDungeonCleared != null)
        {
            onDungeonCleared.Unsubscribe(OpenDoor);
        }
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
        if (other.CompareTag("Player") && entrance != null)
            entrance.SetActive(true);
    }
}
