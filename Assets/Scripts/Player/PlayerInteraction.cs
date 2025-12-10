using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap interactionTilemap;

    [SerializeField] private float verticalOffset = -0.5f;

    private PlayerMovement playerMovement;

    private Vector2 lastDirection = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 feetPosition = transform.position + new Vector3(0, verticalOffset, 0);
        Vector3Int playerCell = grid.WorldToCell(feetPosition);

        // Calculate movement direction
        Vector2 currentInput = new Vector2(playerMovement.horizontalInput, playerMovement.verticalInput);

        Vector3Int facingOffset;

        if (currentInput.sqrMagnitude > 0.1f) // If we are moving
        {
            // Normalize to handle diagonals gracefully for movement
            Vector2 normalized = currentInput.normalized;

            // Update last direction
            lastDirection = normalized;

            if (Mathf.Abs(normalized.x) > Mathf.Abs(normalized.y))
            {
                facingOffset = new Vector3Int(Mathf.RoundToInt(Mathf.Sign(normalized.x)), 0, 0);
            }
            else
            {
                facingOffset = new Vector3Int(0, Mathf.RoundToInt(Mathf.Sign(normalized.y)), 0);
            }
        }
        else
        {
            if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                facingOffset = new Vector3Int(Mathf.RoundToInt(Mathf.Sign(lastDirection.x)), 0, 0);
            }
            else
            {
                facingOffset = new Vector3Int(0, Mathf.RoundToInt(Mathf.Sign(lastDirection.y)), 0);
            }
        }

        Vector3Int targetCell = playerCell + facingOffset;

        // VISUAL DEBUG: Draw a line in the Scene View to show where you are aiming
        Vector3 debugStart = grid.CellToWorld(playerCell) + new Vector3(0.5f, 0.5f, 0);
        Vector3 debugEnd = grid.CellToWorld(targetCell) + new Vector3(0.5f, 0.5f, 0);
        Debug.DrawLine(debugStart, debugEnd, Color.red);

        if (Input.GetMouseButtonDown(1))
        {
            CheckInteraction(targetCell);
        }
    }

    private void CheckInteraction(Vector3Int targetCell)
    {
        // 1. Get the generic TileBase
        TileBase tile = interactionTilemap.GetTile(targetCell);

        // 2. Check if this tile has the 'IInteractable' contract
        // (This works for BOTH EventTile and AnimatedEventTile)
        if (tile is IInteractable interactable)
        {
            interactable.Interact(targetCell, interactionTilemap);
        }
    }
}
