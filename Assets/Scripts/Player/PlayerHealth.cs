using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameEvent onPlayerDeath;
    [SerializeField] private GameEvent onHealthChanged;
    [SerializeField] private EquipmentRegistry registry;
    [SerializeField] private ItemData potion;
    [SerializeField] private GameEvent onPotionCountChanged;

    private int CurrentHealth
    {
        get => GameSession.Instance.runState.currentHealth;
        set => GameSession.Instance.runState.currentHealth = value;
    }

    [Header("I-Frames Settings")]
    [SerializeField] private float iFrameDuration = 1.0f;
    [SerializeField] private float flashInterval = 0.1f;
    private bool isInvincible = false;

    [Header("Heal Settings")]
    [SerializeField] private float healPercentage = 0.3f;
    [SerializeField] private float healInterval = 1f;
    [SerializeField] private int healTicks = 10;

    private SpriteRenderer spriteRenderer;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private Animator anim;

    private RunState RunState => GameSession.Instance.runState;
    private BaseStorage Storage => BaseStorage.Current;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ApplyHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsePotion();
        }
    }

    private void ApplyHealth()
    {
        RunState.armour = registry.armour[RunState.armourIndex];

        if (SceneManager.GetActiveScene().name == "Base")
        {
            RunState.currentHealth = RunState.armour.maxHealth;
        }

        CurrentHealth = RunState.currentHealth;

        onHealthChanged.Raise();
    }

    public void TakeDamage(int damageAmount)
    {
        // Early exit if the player is currently in their i-frame safety window
        if (isInvincible) return;

        ApplyHealthReduction(damageAmount);

        if (CurrentHealth > 0)
        {
            // Gated by hit: Trigger i-frames and standard hit visual flashing
            StartCoroutine(TriggerIFrames());
        }
    }

    public void TakeDamageOverTime(int damageAmount)
    {
        // Even if isInvincible is true, poison still hurts!
        ApplyHealthReduction(damageAmount);

        // Optional: Trigger a unique, subtle visual feedback for status ticks 
        // (like a brief green tint for poison) without starting the i-frame coroutine.
    }

    private void ApplyHealthReduction(int amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        onHealthChanged.Raise();

        if (CurrentHealth <= 0)
            Die();
    }

    private IEnumerator TriggerIFrames()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        while (elapsedTime < iFrameDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    void Die()
    {
        playerInteraction.SyncAnimatorDirection();
        anim.SetTrigger("Death");

        playerMovement.DisableMovement();
    }

     void DeathAnimationCompleted()
    {
        onPlayerDeath.Raise();
    }

    private void UsePotion()
    {
        if (Storage.GetQuantity(potion) > 0 && RunState.currentHealth < RunState.armour.maxHealth)
        {
            StartCoroutine(Heal());
            Storage.RemoveItem(potion, 1);
            onPotionCountChanged.Raise();
        }
    }

    IEnumerator Heal()
    {
        float totalHealAmount = RunState.armour.maxHealth * healPercentage;
        float healAmount = totalHealAmount / healTicks;

        for (int i = 0; i < healTicks; i++)
        {
            RunState.currentHealth = Mathf.Min(
                RunState.currentHealth + Mathf.CeilToInt(healAmount),
                RunState.armour.maxHealth
            );
            onHealthChanged.Raise();
            yield return new WaitForSeconds(healInterval);
        }
        
    }
}