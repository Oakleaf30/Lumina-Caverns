using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    // Call this from your Door script
    public void MoveToRoom(Vector3 roomCenter)
    {
        Vector3 adjustedCenter = new Vector3(roomCenter.x - 0.5f, roomCenter.y - 0.5f, -10f);
        transform.position = adjustedCenter;
    }
}