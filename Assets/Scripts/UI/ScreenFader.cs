using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup group; // Attach a CanvasGroup component to the black panel

    public IEnumerator FadeRoutine(System.Action onBlack)
    {
        // Fade to black
        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime * 2;
            yield return null;
        }

        onBlack.Invoke(); // Move the player now!

        // Fade to clear
        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * 2;
            yield return null;
        }
    }
}