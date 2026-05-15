using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameEvent onTeleportStart;
    [SerializeField] private GameEvent onTeleportEnd;

    [SerializeField] private GameEvent onHoleFell;

    private Animator animator;
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;

    // StringToHash is much faster than passing strings like "MoveX" every frame
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 input = movement.MoveInput;

        // Determine if we are moving
        bool isMoving = input.sqrMagnitude > 0.1f;

        if (input.x > 0.01f) // Moving Right
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < -0.01f) // Moving Left
        {
            spriteRenderer.flipX = true;
        }

        if (isMoving)
        {
            animator.SetFloat(MoveX, input.x);
            animator.SetFloat(MoveY, input.y);
        }

        animator.SetBool(IsMoving, isMoving);
    }

    private void OnEnable()
    {
        onTeleportStart.Subscribe(PauseAnimation);
        onTeleportEnd.Subscribe(ResumeAnimation);

        onHoleFell.Subscribe(HoleFell);
    }

    private void OnDisable()
    {
        onTeleportStart.Unsubscribe(PauseAnimation);
        onTeleportEnd.Unsubscribe(ResumeAnimation);

        onHoleFell.Unsubscribe(HoleFell);
    }

    private void PauseAnimation() => animator.speed = 0;

    private void ResumeAnimation() => animator.speed = 1;

    private void HoleFell()
    {
        PauseAnimation();
    }
}