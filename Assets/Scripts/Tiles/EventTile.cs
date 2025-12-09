using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/Event Tile")]
public class EventTile : InteractableTile
{
    public GameEvent interactionEvent;

    public override void Interact()
    {
        if (interactionEvent != null)
        {
            interactionEvent.Raise();
        }
    }
}