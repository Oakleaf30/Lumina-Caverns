using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        DontDestroyOnLoad(gameObject);

        group = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeRoutine(System.Action onBlack)
    {
        while (group.alpha < 1)
        {
            group.alpha += Time.unscaledDeltaTime * 2;
            yield return null;
        }

        onBlack.Invoke();

        while (group.alpha > 0)
        {
            group.alpha -= Time.unscaledDeltaTime * 2;
            yield return null;
        }
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeRoutine(() =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }
}