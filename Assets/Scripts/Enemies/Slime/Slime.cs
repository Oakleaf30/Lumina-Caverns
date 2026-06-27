using UnityEngine;

// Inherit from EnemyBase instead of MonoBehaviour
public class Slime : EnemyBase
{
    void FixedUpdate()
    {
        if (knockbackTimer > 0)
        {
            return;
        }

        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * data.movementSpeed;
    }
}