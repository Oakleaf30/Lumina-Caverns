using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    private CanvasGroup group; // Attach a CanvasGroup component to the black panel

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        group = GetComponent<CanvasGroup>();
    }

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