using UnityEngine;

public abstract class StationUI : MonoBehaviour
{
    [Header("UI Link")]
    [SerializeField] protected GameObject menuPanel;

    [Header("Game Events")]
    [SerializeField] private GameEvent onStationUsed;
    [SerializeField] private GameEvent onUIOpen;
    [SerializeField] private GameEvent onUIClose;

    // Standard Unity lifecycle hooks handle the event setup automatically
    protected virtual void OnEnable()
    {
        onStationUsed.Subscribe(OpenMenu);
    }

    protected virtual void OnDisable()
    {
        onStationUsed.Unsubscribe(OpenMenu);

        CloseMenu();
    }

    // Opens the screen and pauses the game
    protected virtual void OpenMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        Time.timeScale = 0f;
        onUIOpen.Raise();
    }

    // Closes the screen, unpauses the game, and notifies other systems
    public virtual void CloseMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        onUIClose.Raise();
    }
}