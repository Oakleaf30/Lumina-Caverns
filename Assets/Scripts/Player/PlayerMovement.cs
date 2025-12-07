using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 MoveInput { get; private set; }

    public float moveSpeed = 5f;

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

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
    }

    void FixedUpdate()
    {
        Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
        moveVector.Normalize();
        rb.linearVelocity = moveVector * moveSpeed;
    }
}
