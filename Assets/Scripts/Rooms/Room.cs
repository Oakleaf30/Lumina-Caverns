using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [Header("Teleportation References")]
    // Leave a slot empty if the room has no door on that side
    public Door topDoor;
    public Door bottomDoor;
    public Door leftDoor;
    public Door rightDoor;

    public bool hasTopDoor => topDoor != null;
    public bool hasBottomDoor => bottomDoor != null;
    public bool hasLeftDoor => leftDoor != null;
    public bool hasRightDoor => rightDoor != null;

    public int RoomID { get; private set; }

    [Header("Room Settings")]
    public bool isLadderRoom;
    public bool isDungeon;
    public bool isChestRoom;


    public void InitializeRoom(int id)
    {
        RoomID = id;
    }

    [Space(10)]
    public Vector2 size = new Vector2(17f, 17f);
    public Vector2 centerOffset = Vector2.zero;
    private Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(centerOffset, size);

        Gizmos.matrix = oldMatrix;
    }


    protected Tilemap pathMap;
    protected Tilemap wallTopMap;

    void Awake()
    {
        pathMap = transform.Find("Floor 2").GetComponent<Tilemap>();
        wallTopMap = transform.Find("Wall Extras/Ghost Wall Top").GetComponent<Tilemap>();
    }

    public void ApplyDungeonEntrance(Vector3 doorWorldPos, int reach, TileBase pathTile, TileBase wallTile)
    {
        Vector3Int center = pathMap.WorldToCell(doorWorldPos);

        for (int x = -reach; x <= reach; x++)
            for (int y = -reach; y <= reach; y++)
            {
                Vector3Int cell = center + new Vector3Int(x, y, 0);

                if (pathMap.GetTile(cell) != null)
                    pathMap.SetTile(cell, pathTile);

                if (wallTopMap.GetTile(cell) != null)
                    wallTopMap.SetTile(cell, wallTile);
            }
    }
}