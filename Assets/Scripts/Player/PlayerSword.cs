using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private GameEvent onSwordSwing;

    private Animator anim;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    [Header("Mining Mechanics")]
    [SerializeField] private float swingCooldown = 0.4f;

    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= lastSwingTime + swingCooldown && !anim.GetBool("IsSwimming"))
        {
            SwingSword();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.ResetTrigger("Sword");
        }
    }

    void SwingSword()
    {
        lastSwingTime = Time.time;

        Vector2 lookDir = playerInteraction.GetLastDirection();
        anim.SetFloat("MoveX", lookDir.x);
        anim.SetFloat("MoveY", lookDir.y);

        anim.SetTrigger("Sword");
        playerMovement.DisableMovement();

        onSwordSwing.Raise();
    }
}