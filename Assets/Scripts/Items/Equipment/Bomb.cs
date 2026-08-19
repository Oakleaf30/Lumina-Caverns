using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private int explosionDamage = 25;

    [Header("Explosion Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite explosionSprite;
    [SerializeField] private float fadeDuration = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        
        spriteRenderer.sprite = explosionSprite;
        StartCoroutine(FadeOut());

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layer);

        foreach (Collider2D targetCollider in hitTargets)
        {
            if (targetCollider.TryGetComponent(out OreNode node))
            {
                node.BreakNode();
            }

            if (targetCollider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.Die();
            }

            if (targetCollider.TryGetComponent(out PlayerHealth player))
            {
                player.TakeDamage(explosionDamage);
            }

        }
    }

    IEnumerator FadeOut()
    {
        Color color = spriteRenderer.color;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a = 1f - (elapsed / fadeDuration);
            spriteRenderer.color = color;

            yield return null;
        }

        color.a = 0f;
        spriteRenderer.color = color;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Filled, semi-transparent so you can see overlap with nodes/enemies
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, explosionRadius);

        // Solid outline for a clean edge reference
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
