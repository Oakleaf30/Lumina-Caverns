using UnityEngine.Tilemaps;
using UnityEngine;

public interface IInteractable
{
    void Interact(Vector3Int position, Tilemap tilemap);
}