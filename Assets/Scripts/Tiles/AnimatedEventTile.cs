using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Animated Event Tile")]
public class AnimatedEventTile : AnimatedTile, IInteractable // Inherits AnimatedTile!
{
    public Vector3IntEvent interactionEvent;

    public void Interact(Vector3Int position, Tilemap tilemap)
    {
        // NO ERROR NOW: You are calling the Raise(Vector3Int) method on the new asset type
        if (interactionEvent != null)
        {
            interactionEvent.Raise(position);
        }
    }

    public void SetAnimationState(Vector3Int position, UnityEngine.Tilemaps.Tilemap tilemap, bool isPaused)
    {
        // FIX: Check if the Tilemap is destroyed before using it.
        if (tilemap == null)
        {
            return;
        }

        TileAnimationFlags flags = isPaused ? TileAnimationFlags.PauseAnimation : TileAnimationFlags.None;

        // This command is now safe because we checked for null:
        tilemap.SetTileAnimationFlags(position, flags);
    }
}