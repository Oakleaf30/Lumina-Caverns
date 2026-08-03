using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameEvent onPlayerDeath;
    [SerializeField] private EquipmentRegistry registry;

    private int currentHealth;

    [Header("I-Frames Settings")]
    [SerializeField] private float iFrameDuration = 1.0f;
    [SerializeField] private float flashInterval = 0.1f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private Animator anim;

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

    private void ApplyHealth()
    {
        GameSession.Instance.runState.armour = registry.armour[GameSession.Instance.runState.armourIndex];

        if (SceneManager.GetActiveScene().name == "Base")
        {
            currentHealth = GameSession.Instance.runState.armour.maxHealth;
        } else
        {
            currentHealth = GameSession.Instance.runState.currentHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Early exit if the player is currently in their i-frame safety window
        if (isInvincible) return;

        ApplyHealthReduction(damageAmount);

        if (currentHealth > 0)
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
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Broadcast to UI layer via your decoupled event system
        // onPlayerHealthChanged?.Raise();

        if (currentHealth <= 0)
        {
            Die();
        }
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
}