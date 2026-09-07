using UnityEngine;

public class Skeleton : EnemyBase
{
    [Header("Behaviour Tuning")]
    [SerializeField] private float decisionInterval = 0.3f; // how often it re-picks a direction

    private Vector2 currentDirection;
    private float decisionTimer;

    protected override void Update()
    {
        base.Update();

        if (frozen)
        {
            return;
        }

        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            PickDirection();
            decisionTimer = decisionInterval;
        }

        transform.position += (Vector3)currentDirection * data.movementSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Compares the absolute distance to the player on X vs Y and commits
    /// to a single cardinal direction along whichever axis is larger.
    /// </summary>
    private void PickDirection()
    {
        Vector2 diff = player.transform.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            currentDirection = new Vector2(Mathf.Sign(diff.x), 0f);
        }
        else
        {
            currentDirection = new Vector2(0f, Mathf.Sign(diff.y));
        }
    }
}