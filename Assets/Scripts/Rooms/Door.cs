using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public Door connectedDoor;
    public Transform exitPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && connectedDoor != null)
        {
            // 1. Grab the movement script (e.g., PlayerController)
            var movement = collision.GetComponent<PlayerMovement>();
            var rb = collision.GetComponent<Rigidbody2D>();
            var anim = collision.GetComponent<Animator>();

            // 2. Disable movement immediately before the fade starts
            if (movement != null) movement.enabled = false;
            rb.linearVelocity = Vector3.zero;
            anim.speed = 0;

            // Start screen fade
            StartCoroutine(ScreenFader.Instance.FadeRoutine(() =>
            {
                // This part runs ONLY when the screen is fully black
                collision.transform.position = connectedDoor.exitPoint.position;

                Vector3 newRoomPos = connectedDoor.transform.parent.position;
                Camera.main.GetComponent<RoomCamera>().MoveToRoom(newRoomPos);

                if (movement != null) movement.enabled = true;
                anim.speed = 1;
            }));
        }
    }
}
