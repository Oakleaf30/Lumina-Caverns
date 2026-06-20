using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private StatusEffectData effectToApply; // Optional! Can be null.

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Deal the flat damage through the health system
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(data.contactDamage);
            }

            // 2. If this enemy has a status effect, hand it to the player's status handler
            if (effectToApply != null)
            {
                PlayerStatusHandler statusHandler = collision.GetComponent<PlayerStatusHandler>();
                if (statusHandler != null)
                {
                    statusHandler.ApplyStatus(effectToApply);
                }
            }
        }
    }
}