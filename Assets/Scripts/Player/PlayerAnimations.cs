using UnityEngine;
using System.Collections; // Required for IEnumerator and Coroutines

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameEvent onTeleportStart;
    [SerializeField] private GameEvent onTeleportEnd;

    [SerializeField] private GameEvent onHoleFell;
    [SerializeField] private GameEvent onRespawn;

    [SerializeField] private GameEvent onWaterEnter;
    [SerializeField] private GameEvent onWaterExit;

    private Animator animator;
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;

    // StringToHash is much faster than passing strings like "MoveX" every frame
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsSwimming = Animator.StringToHash("isSwimming");

    // NEW: Variables to handle the 1-frame stop delay
    private Coroutine stopCoroutine;
    private bool isActuallyMoving = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 input = movement.MoveInput;

        // Check if there is physical input this exact frame
        bool hasInput = input.sqrMagnitude > 0.1f;

        if (input.x > 0.01f) // Moving Right
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < -0.01f) // Moving Left
        {
            spriteRenderer.flipX = true;
        }

        if (hasInput)
        {
            // Update blend tree parameters
            animator.SetFloat(MoveX, input.x);
            animator.SetFloat(MoveY, input.y);

            // If a stop was scheduled, cancel it because we have input again
            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
                stopCoroutine = null;
            }

            // Apply movement animation state
            if (!isActuallyMoving)
            {
                isActuallyMoving = true;
                animator.SetBool(IsMoving, true);
            }
        }
        else if (isActuallyMoving && stopCoroutine == null)
        {
            // Input dropped below 0.1f, but wait 1 frame before officially stopping
            // to bridge the gap of a direction switch.
            stopCoroutine = StartCoroutine(WaitFrameToStop());
        }
    }

    private IEnumerator WaitFrameToStop()
    {
        // 0.05 seconds is ~3 frames. Imperceptible to the eye when stopping,
        // but exactly long enough to cover human fingers switching keys.
        yield return new WaitForSeconds(0.05f);

        // After 50ms, if this coroutine wasn't canceled by new input, stop the animation
        isActuallyMoving = false;
        animator.SetBool(IsMoving, false);
        stopCoroutine = null;
    }

    private void OnEnable()
    {
        onTeleportStart.Subscribe(PauseAnimation);
        onTeleportEnd.Subscribe(ResumeAnimation);

        onHoleFell.Subscribe(PauseAnimation);
        onRespawn.Subscribe(ResumeAnimation);

        onWaterEnter.Subscribe(SetSwimmingTrue);
        onWaterExit.Subscribe(SetSwimmingFalse);
    }

    private void OnDisable()
    {
        onTeleportStart.Unsubscribe(PauseAnimation);
        onTeleportEnd.Unsubscribe(ResumeAnimation);

        onHoleFell.Unsubscribe(PauseAnimation);
        onRespawn.Unsubscribe(ResumeAnimation);

        onWaterEnter.Unsubscribe(SetSwimmingTrue);
        onWaterExit.Unsubscribe(SetSwimmingFalse);
    }

    private void PauseAnimation() => animator.speed = 0;

    private void ResumeAnimation() => animator.speed = 1;

    private void SetSwimmingTrue() => animator.SetBool("IsSwimming", true);
    private void SetSwimmingFalse() => animator.SetBool("IsSwimming", false);
}