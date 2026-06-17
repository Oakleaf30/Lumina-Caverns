using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameEvent onPlayerDeath;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("I-Frames Settings")]
    [SerializeField] private float iFrameDuration = 1.0f;
    [SerializeField] private float flashInterval = 0.1f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private Animator anim;

    // Optional: Hook into your decoupled event system
    // [SerializeField] private GameEvent onPlayerDamaged;
    // [SerializeField] private GameEvent onPlayerDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
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