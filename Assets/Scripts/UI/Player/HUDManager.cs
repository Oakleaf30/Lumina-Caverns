using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameEvent onUIOpen;
    [SerializeField] private GameEvent onUIClose;

    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        onUIOpen.Subscribe(Hide);
        onUIClose.Subscribe(Show);
    }

    private void OnDisable()
    {
        onUIOpen.Unsubscribe(Hide);
        onUIClose.Unsubscribe(Show);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
