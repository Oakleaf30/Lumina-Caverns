using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private GameEvent onPlayerDied;

    private void OnEnable()
    {
        onPlayerDied.Subscribe(HandlePlayerDeathSequence);
    }

    private void OnDisable()
    {
        onPlayerDied.Unsubscribe(HandlePlayerDeathSequence);
    }

    private void HandlePlayerDeathSequence()
    {
        SceneLoader.Instance.StartCoroutine(ScreenFader.Instance.FadeRoutine(() =>
        {
            SceneManager.LoadScene("Base");
        }));
    }
}
