using UnityEngine;

public class SpawnAnchor : MonoBehaviour
{
    public float spawnRadius = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}