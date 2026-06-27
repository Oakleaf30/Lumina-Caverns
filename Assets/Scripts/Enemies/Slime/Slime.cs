using UnityEngine;

public class Slime : EnemyBase
{
    protected override void Update()
    {
        base.Update();

        if (frozen)
        {
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * data.movementSpeed * Time.deltaTime;
    }
}