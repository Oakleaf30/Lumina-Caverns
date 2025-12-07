using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
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
        // 1. Get the data from the movement script
        Vector2 input = movement.MoveInput;

        // 2. Determine if we are moving
        bool isMoving = input.sqrMagnitude > 0.1f;

        if (input.x > 0.01f) // Moving Right
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < -0.01f) // Moving Left
        {
            spriteRenderer.flipX = true;
        }

        // 3. Update the Animator Parameters
        if (isMoving)
        {
            // Only update the facing direction when we are actually moving.
            // This prevents the character from snapping to "Default" when you stop.
            animator.SetFloat(MoveX, input.x);
            animator.SetFloat(MoveY, input.y);
        }

        animator.SetBool(IsMoving, isMoving);
    }
}