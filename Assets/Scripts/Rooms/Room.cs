using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Generator Data")]
    public bool hasTopDoor;
    public bool hasBottomDoor;
    public bool hasLeftDoor;
    public bool hasRightDoor;

    [Header("Teleportation References")]
    // Drag your "Logic Door" child objects into these slots in the Inspector
    public Door topDoor;
    public Door bottomDoor;
    public Door leftDoor;
    public Door rightDoor;

    public int RoomID { get; private set; }
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