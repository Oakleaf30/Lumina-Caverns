using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private GameEvent onPickaxeSwing;

    private Animator anim;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    [SerializeField] private float swingCooldown = 0.4f;
    private float lastSwingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastSwingTime + swingCooldown && !anim.GetBool("IsSwimming"))
        {
            SwingPickaxe();
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.ResetTrigger("Swing");
        }
    }

    private void SwingPickaxe()
    {
        lastSwingTime = Time.time;

        Vector2 lookDir = playerInteraction.GetLastDirection();

        anim.SetFloat("MoveX", lookDir.x);
        anim.SetFloat("MoveY", lookDir.y);

        anim.SetTrigger("Swing");

        playerMovement.DisableMovement();

        onPickaxeSwing.Raise();
    }
}