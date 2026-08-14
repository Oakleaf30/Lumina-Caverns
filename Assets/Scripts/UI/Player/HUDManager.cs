using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameEvent onUIOpen;
    [SerializeField] private GameEvent onUIClose;

    [SerializeField] private GameObject HUDContainer;

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

    private void Hide()
    {
        HUDContainer.SetActive(false);
    }

    private void Show()
    {
        HUDContainer.SetActive(true);
    }
}
