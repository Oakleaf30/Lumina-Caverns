using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 MoveInput { get; private set; }

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

    [SerializeField] private float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(horizontalInput, verticalInput);

        CheckPlayerStep();
    }

    void FixedUpdate()
    {
        Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
        moveVector.Normalize();
        rb.linearVelocity = moveVector * moveSpeed;
    }

    // CRUMBLE TILE LOGIC ----------------------------------------------------------------------------------------

    private CrumbleManager activeCrumbleManager;
    private Grid activeGrid;
    private Vector3Int previousCell;

    void CheckPlayerStep()
    {
        if (activeGrid != null && activeCrumbleManager != null)
        {
            Vector3Int currentCell = activeGrid.WorldToCell(transform.position);

            if (currentCell != previousCell)
            {
                activeCrumbleManager.ProcessPlayerStep(currentCell);
                previousCell = currentCell;

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When we enter a room boundary, store its components
        activeGrid = other.GetComponent<Grid>();
        activeCrumbleManager = other.GetComponent<CrumbleManager>();
    }
}
