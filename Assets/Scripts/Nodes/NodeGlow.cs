using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NodeGlow : MonoBehaviour
{
    [Header("Glow Settings (fixed values)")]
    [SerializeField] private float glowRadius = 1.25f;
    [SerializeField] private float glowIntensity = 0.6f;
    [SerializeField] private Color glowColor = new Color(0.6f, 0.8f, 1f); // soft cyan-ish "magic detection" tint

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float holdDuration = 2.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private Light2D glowLight;

    private void Awake()
    {
        glowLight = gameObject.AddComponent<Light2D>();
        glowLight.lightType = Light2D.LightType.Point;
        glowLight.pointLightOuterRadius = glowRadius;
        glowLight.pointLightInnerRadius = glowRadius * 0.4f;
        glowLight.color = glowColor;
        glowLight.intensity = 0f;
    }

    private void Start()
    {
        StartCoroutine(GlowSequence());
    }

    private IEnumerator GlowSequence()
    {
        yield return Fade(0f, glowIntensity, fadeInDuration);
        yield return new WaitForSeconds(holdDuration);
        yield return Fade(glowIntensity, 0f, fadeOutDuration);
        Destroy(gameObject);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            glowLight.intensity = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        glowLight.intensity = to;
    }

    public static void SpawnAt(Vector3 worldPosition)
    {
        GameObject glowObj = new GameObject("NodeGlow");
        glowObj.transform.position = worldPosition;
        glowObj.AddComponent<NodeGlow>();
    }
}