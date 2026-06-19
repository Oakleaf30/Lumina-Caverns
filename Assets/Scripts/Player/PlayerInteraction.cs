using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRadius = 0.4f;
    [SerializeField] private float interactDistance = 1.0f; // How far the line stretches
    [SerializeField] private float verticalOffset = -0.5f;  // Down to the player's feet
    [SerializeField] private LayerMask interactableLayer;

    private PlayerMovement playerMovement;
    private Vector2 lastDirection = Vector2.down; // Default facing direction

    private Animator animator;

    public Vector3 TargetCentre { get; private set; }

    // ADD THIS: A public helper so your Mining script can easily get the world center of that cell
    public Vector3 GetTargetCellCenterWorld()
    {
        return TargetCentre;
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Track direction (simplified)
        Vector2 currentInput = new Vector2(playerMovement.horizontalInput, playerMovement.verticalInput);
        if (currentInput.sqrMagnitude > 0.1f)
        {
            // Lock it to 4 directions just like your old script did!
            if (Mathf.Abs(currentInput.x) > Mathf.Abs(currentInput.y))
                lastDirection = new Vector2(Mathf.Sign(currentInput.x), 0);
            else
                lastDirection = new Vector2(0, Mathf.Sign(currentInput.y));
        }

        // 2. Calculate the exact point in space (Feet + Direction)
        Vector3 feetPosition = transform.position + new Vector3(0, verticalOffset, 0);
        TargetCentre = feetPosition + (Vector3)(lastDirection * interactDistance);

        // 3. YOUR VISUAL DEBUG LINE
        Debug.DrawLine(feetPosition, TargetCentre, Color.red);

        // 4. Fire the interaction exactly at the end of the red line
        if (Input.GetMouseButtonDown(1))
        {
            CheckInteraction(TargetCentre);
        }
    }

    private void CheckInteraction(Vector3 targetPosition)
    {
        // Cast the circle exactly where the red line ends
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, interactRadius, interactableLayer);

        if (hit != null && hit.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact();
        }
    }

    // BONUS: Draws the actual circle in the editor so you can perfectly size it
    private void OnDrawGizmosSelected()
    {
        Vector3 feetPosition = transform.position + new Vector3(0, verticalOffset, 0);

        // Use lastDirection if playing, otherwise default to down so you can see it in the editor
        Vector3 direction = Application.isPlaying ? (Vector3)lastDirection : Vector3.down;
        Vector3 targetCenter = feetPosition + (direction * interactDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCenter, interactRadius);
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }

    public void SyncAnimatorDirection()
    {
        Vector2 lookDir = GetLastDirection().normalized;
        animator.SetFloat("MoveX", lookDir.x);
        animator.SetFloat("MoveY", lookDir.y);
    }
}