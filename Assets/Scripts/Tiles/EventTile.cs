using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Event Tile")]
public class EventTile : Tile, IInteractable
{
    public GameEvent interactionEvent;

    public void Interact(Vector3Int position, Tilemap tilemap)
    {
        if (interactionEvent != null)
        {
            interactionEvent.Raise();
        }
    }
}