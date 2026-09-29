using UnityEngine;

public class SpawnAnchor : MonoBehaviour
{
    public float spawnRadius = 2f;
    public int pairGroup = -1; // -1 = standalone anchor. Anchors sharing the same non-negative value are alternates of each other.

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    [SerializeField] private GameEvent onLastEnemyDefeated;

    private void OnEnable()
    {
        onLastEnemyDefeated.Subscribe(FindPlayer);
    }

    private void OnDisable()
    {
        onLastEnemyDefeated.Unsubscribe(FindPlayer);
    }

    private void FindPlayer()
    {
        var ladder = GameObject.FindGameObjectWithTag("Ladder").GetComponent<LadderTile>();
        ladder.CompareDistances(transform.position);
    }
}