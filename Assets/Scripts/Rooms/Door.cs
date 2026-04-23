using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public Door connectedDoor;
    public Transform exitPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && connectedDoor != null)
        {
            // Teleport the player to the other door's exit point
            collision.transform.position = connectedDoor.exitPoint.position;

            // Optional: Handle camera transition here
            Debug.Log("Teleported to " + connectedDoor.gameObject.name);
        }
    }
}
