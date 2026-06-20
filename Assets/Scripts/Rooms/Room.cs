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

    public void InitializeRoom(int id)
    {
        RoomID = id;
    }
}