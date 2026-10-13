using UnityEngine;

public class LightReveal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField, Range(0.5f, 1f)] private float revealFraction = 0.9f;
    [SerializeField] private float fadeSpeed = 8f;

    private float alpha;

    private void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var vision = PlayerVision.Instance;
        if (vision == null) return;

        float dist = Vector2.Distance(transform.position, vision.transform.position);
        float target = dist <= vision.Radius * revealFraction ? 1f : 0f;
        alpha = Mathf.MoveTowards(alpha, target, fadeSpeed * Time.deltaTime);

        var c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}