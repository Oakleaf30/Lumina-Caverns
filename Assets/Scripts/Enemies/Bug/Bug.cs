using UnityEngine;

public class Bug : EnemyBase
{
    [SerializeField] private float moveSpeed = 3f;

    private Vector2 direction;

    private static readonly Vector2[] CardinalDirections =
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    protected override void Awake()
    {
        base.Awake();
        direction = CardinalDirections[Random.Range(0, CardinalDirections.Length)];
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(direction, normal);
        direction = SnapToCardinal(reflected);
    }

    private Vector2 SnapToCardinal(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return v.x > 0 ? Vector2.right : Vector2.left;
        else
            return v.y > 0 ? Vector2.up : Vector2.down;
    }
}