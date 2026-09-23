using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class PlayerVision : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float baseRadius = 3.5f;
    [SerializeField] private float baseIntensity = 1f;
    [SerializeField] private float baseFalloffStrength = 0.5f; // soft edge amount

    private Light2D visionLight;

    private void Awake()
    {
        visionLight = GetComponent<Light2D>();
        ApplyBaseValues();
    }

    private void ApplyBaseValues()
    {
        visionLight.lightType = Light2D.LightType.Point;
        visionLight.pointLightOuterRadius = baseRadius;
        visionLight.pointLightInnerRadius = 0;
        visionLight.intensity = baseIntensity;
    }

    public void SetRadius(float newRadius)
    {
        visionLight.pointLightOuterRadius = newRadius;
        visionLight.pointLightInnerRadius = newRadius * (1f - baseFalloffStrength);
    }

    public void SetIntensity(float newIntensity)
    {
        visionLight.intensity = newIntensity;
    }
}