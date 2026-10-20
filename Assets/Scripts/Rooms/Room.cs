using UnityEngine;

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
    public bool isDungeon;
    public bool isChestRoom;

    public void InitializeRoom(int id)
    {
        RoomID = id;
    }

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
}