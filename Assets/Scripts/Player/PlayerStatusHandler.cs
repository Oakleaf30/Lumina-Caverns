using System.Collections;
using UnityEngine;

public class PlayerStatusHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    // Tracks how many active status effects are currently slowing the player down
    private int activeSlowCount = 0;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ApplyStatus(StatusEffectData data)
    {
        StartCoroutine(ProcessStatusRoutine(data));
    }

    private IEnumerator ProcessStatusRoutine(StatusEffectData data)
    {
        Debug.Log($"Applied status effect: {data.effectName}");

        bool appliedSlowToMovement = false;

        // 1. Check if this status effect actually has a movement slow (multiplier < 1.0)
        if (playerMovement != null && data.speedMultiplier < 1f)
        {
            // Capped Scope Protection: Only apply to PlayerMovement if there isn't already a slow active
            if (activeSlowCount == 0)
            {
                playerMovement.ApplySpeedMultiplier(data.speedMultiplier);
                appliedSlowToMovement = true; // Remember that THIS specific coroutine applied the slow
            }

            // Increment the counter regardless, tracking that a "slowing" hazard is acting on the player
            activeSlowCount++;
        }

        float elapsedTime = 0f;
        float nextTickTime = 0f;

        // 2. Loop through the duration (Safe for all concurrent effects!)
        while (elapsedTime < data.duration)
        {
            // Handle Damage Over Time (DOT) ticks (e.g., Poison ticks)
            if (data.damagePerTick > 0 && elapsedTime >= nextTickTime)
            {
                // Matches your direct damage method entry point
                playerHealth.TakeDamageOverTime(data.damagePerTick);
                nextTickTime += data.tickInterval;
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // 3. Clean up when this specific duration ends
        if (playerMovement != null && data.speedMultiplier < 1f)
        {
            activeSlowCount--;

            // Only remove the multiplier if this specific instance was the one allowed to apply it,
            // AND there are no other remaining slowing statuses waiting in line.
            if (appliedSlowToMovement && activeSlowCount == 0)
            {
                playerMovement.RemoveSpeedMultiplier(data.speedMultiplier);
            }
        }
    }
}