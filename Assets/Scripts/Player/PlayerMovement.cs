using UnityEngine;
using Lumina.VisualFX;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameEvent onTeleportStart;
    [SerializeField] private GameEvent onTeleportEnd;

    [SerializeField] private GameEvent onHoleFell;
    [SerializeField] private GameEvent onRespawn;

    private Rigidbody2D rb;

    private bool isFrozen = false;
    private bool isFalling = false;

    private Vector3 spawnPoint;

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
        if (isFrozen) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(horizontalInput, verticalInput);

        //CheckPlayerStep();
    }

    void FixedUpdate()
    {
        if (isFrozen) return;

        Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
        moveVector.Normalize();
        rb.linearVelocity = moveVector * moveSpeed;
    }

    private void OnEnable()
    {
        onTeleportStart.Subscribe(DisableMovement);
        onTeleportEnd.Subscribe(EnableMovement);

        onHoleFell.Subscribe(HoleFell);
    }

    private void OnDisable()
    {
        onTeleportStart.Unsubscribe(DisableMovement);
        onTeleportEnd.Unsubscribe(EnableMovement);

        onHoleFell.Unsubscribe(HoleFell);
    }

    private void DisableMovement()
    {
        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void EnableMovement() => isFrozen = false;

    private void HoleFell()
    {
        if (isFalling) return;

        StartCoroutine(FallAndRespawn());
    }

    IEnumerator FallAndRespawn()
    {
        isFalling = true;

        float duration = 2;
        float spins = 2;
        float direction = -1f;

        if (MoveInput.x < -0.1f) direction = 1f;

        float finalSpins = spins * direction;

        bool isBackFall = MoveInput.y < -0.1f;
        if (isBackFall)
        {
            float fallDistance = 1;
            Vector3 targetPos = transform.position + (Vector3.down * fallDistance);
            StartCoroutine(VFX.MoveToTarget(transform, targetPos, 2));
        }

        DisableMovement();
        StartCoroutine(VFX.ChangeSize(transform, duration, transform.localScale, Vector3.zero));
        StartCoroutine(VFX.Spin(transform, duration, finalSpins));

        yield return new WaitForSeconds(duration);

        Respawn();
    }

    public void SetRespawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    void Respawn()
    {
        StartCoroutine(ScreenFader.Instance.FadeRoutine(() =>
        {
            transform.position = spawnPoint;
            transform.localScale = Vector3.one;

            onRespawn.Raise();

            isFalling = false;
            EnableMovement();
        }));

        
    }

    // CRUMBLE TILE LOGIC ----------------------------------------------------------------------------------------

    //private CrumbleManager activeCrumbleManager;
    //private Grid activeGrid;
    //private Vector3Int previousCell;

    //void CheckPlayerStep()
    //{
    //    if (activeGrid != null && activeCrumbleManager != null)
    //    {
    //        Vector3Int currentCell = activeGrid.WorldToCell(transform.position);

    //        if (currentCell != previousCell)
    //        {
    //            activeCrumbleManager.ProcessPlayerStep(currentCell);
    //            previousCell = currentCell;

    //        }
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    // When we enter a room boundary, store its components
    //    activeGrid = other.GetComponent<Grid>();
    //    activeCrumbleManager = other.GetComponent<CrumbleManager>();
    //}
}
