using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Door Configuration (Default Rotation)")]
    public bool hasTopDoor;
    public bool hasBottomDoor;
    public bool hasLeftDoor;
    public bool hasRightDoor;

    // A helper to check doors based on rotation
    public bool HasDoor(string direction, int rotation90DegSteps)
    {
        // We simulate rotating the data to match the object's rotation
        // 0 = 0 deg, 1 = -90 deg, 2 = 180 deg, 3 = -270 deg

        bool t = hasTopDoor;
        bool b = hasBottomDoor;
        bool l = hasLeftDoor;
        bool r = hasRightDoor;

        // Rotate the data "clockwise" for every 90 degree step
        for (int i = 0; i < rotation90DegSteps; i++)
        {
            bool tempTop = l;
            l = b;
            b = r;
            r = t;
            t = tempTop;
        }

        if (direction == "Top") return t;
        if (direction == "Bottom") return b;
        if (direction == "Left") return l;
        if (direction == "Right") return r;

        return false;
    }
}