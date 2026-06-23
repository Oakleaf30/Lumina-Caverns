using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Calculate direction exactly like you did
        Vector3 direction = (player.position - transform.position).normalized;

        // 2. Feed it into the Rigidbody's velocity instead of transform.position
        // Note: In newer Unity versions, use '.linearVelocity'. In older versions, use '.velocity'.
        rb.linearVelocity = direction * data.movementSpeed;
    }
}
