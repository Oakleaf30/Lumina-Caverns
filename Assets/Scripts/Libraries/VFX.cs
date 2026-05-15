using System.Collections;
using UnityEngine;

namespace Lumina.VisualFX
{
    public static class VFX
    {
        public static IEnumerator ChangeSize(Transform transform, float duration, Vector3 startScale, Vector3 targetScale)
        {
            transform.localScale = startScale;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        public static IEnumerator Spin(Transform transform, float duration, float spins)
        {
            float totalRotation = spins * 360;

            float elapsed = 0f;
            Quaternion startRot = transform.rotation;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Rotate smoothly based on normalized time
                transform.rotation = startRot * Quaternion.Euler(0, 0, totalRotation * t);

                yield return null;
            }

            // Ensure it finishes exactly upright
            transform.rotation = startRot * Quaternion.Euler(0, 0, totalRotation);
        }

        public static IEnumerator MoveToTarget(Transform transform, Vector3 targetPosition, float duration)
        {
            Vector3 startPos = transform.position;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, targetPosition, time / duration);
                yield return null;
            }

            transform.position = targetPosition;
        }
    }
}