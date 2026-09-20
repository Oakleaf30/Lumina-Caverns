using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float scale;
    [SerializeField] private float verticalOffset;
    [SerializeField] private int damage;
    [SerializeField] private float knockbackForce;

    private Transform player;
    private HashSet<EnemyBase> alreadyHit = new HashSet<EnemyBase>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(ChangeSize());
    }

    private void Update()
    {
        transform.position = player.position + new Vector3(0, verticalOffset, 0); ;
    }

    private IEnumerator ChangeSize()
    {
        transform.localScale = Vector3.zero;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * scale, t);
            yield return null;
        }

        transform.localScale = Vector3.one * scale;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy) && alreadyHit.Add(enemy))
        {
            Vector2 knockbackDir = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;

            HitInfo hit = new HitInfo
            {
                damage = damage,
                knockback = knockbackDir * knockbackForce
            };

            collision.GetComponent<EnemyBase>().TakeDamage(hit);
        }
    }
}
