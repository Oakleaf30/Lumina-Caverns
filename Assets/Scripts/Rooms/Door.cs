using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameEvent onTeleportStart;
    [SerializeField] private GameEvent onTeleportEnd;

    [HideInInspector] public Door connectedDoor;
    public Transform exitPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && connectedDoor != null)
{
            onTeleportStart.Raise();

            // Start screen fade
            StartCoroutine(ScreenFader.Instance.FadeRoutine(() =>
            {
                other.transform.position = connectedDoor.exitPoint.position;
                Vector3 newRoomPos = connectedDoor.transform.parent.position;
                Camera.main.GetComponent<RoomCamera>().MoveToRoom(newRoomPos);

                // 4. Tell everyone the teleport is finished
                onTeleportEnd.Raise();
            }));
        }
    }
}
