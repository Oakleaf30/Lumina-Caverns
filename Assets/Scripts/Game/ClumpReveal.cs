using UnityEngine;

public class ClumpReveal : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1f)] private float revealFraction = 0.9f;
    [SerializeField] private float fadeSpeed = 6f;

    private SpriteRenderer[] renderers = new SpriteRenderer[0];
    private float alpha;

    public void Refresh()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        SetAlpha(alpha);
    }

    private void Update()
    {
        var vision = PlayerVision.Instance;
        if (vision == null) return;

        float dist = Vector2.Distance(transform.position, vision.transform.position);
        float target = dist <= vision.Radius * revealFraction ? 1f : 0f;
        alpha = Mathf.MoveTowards(alpha, target, fadeSpeed * Time.deltaTime);
        SetAlpha(alpha);
    }

    private void SetAlpha(float a)
    {
        foreach (var r in renderers)
        {
            if (r == null) continue; // mined and destroyed
            var c = r.color;
            c.a = a;
            r.color = c;
        }
    }
}