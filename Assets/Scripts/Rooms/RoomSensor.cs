using UnityEngine;

public class RoomSensor : MonoBehaviour
{
    private Room parentRoom;

    private void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.UpdateCurrentRoom(parentRoom.RoomID);
        }
    }
}