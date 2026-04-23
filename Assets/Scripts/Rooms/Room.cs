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

    // You can keep this helper if you want to check door status in other scripts!
    public bool HasDoor(string direction)
    {
        if (direction == "Top") return hasTopDoor;
        if (direction == "Bottom") return hasBottomDoor;
        if (direction == "Left") return hasLeftDoor;
        if (direction == "Right") return hasRightDoor;
        return false;
    }
}