using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("I-Frames Settings")]
    [SerializeField] private float iFrameDuration = 1.0f;
    [SerializeField] private float flashInterval = 0.1f;
    private bool isInvincible = false;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Optional: Hook into your decoupled event system
    // [SerializeField] private GameEvent onPlayerDamaged;
    // [SerializeField] private GameEvent onPlayerDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        Debug.Log($"Health modified. Current Health: {currentHealth}");

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
            // Toggle sprite visibility to create a flashing effect
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        // Ensure the sprite is fully visible when i-frames finish
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Die");
    }
}