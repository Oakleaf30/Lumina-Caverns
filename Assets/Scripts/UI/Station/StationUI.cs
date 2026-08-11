using UnityEngine;

public abstract class StationUI : MonoBehaviour
{
    [Header("UI Link")]
    [SerializeField] protected GameObject menuPanel;

    [Header("Game Events")]
    [SerializeField] private GameEvent onStationUsed;
    [SerializeField] private GameEvent onStationClosed;

    // Standard Unity lifecycle hooks handle the event setup automatically
    protected virtual void OnEnable()
    {
        if (onStationUsed != null)
            onStationUsed.Subscribe(OpenMenu);
    }

    protected virtual void OnDisable()
    {
        if (onStationUsed != null)
            onStationUsed.Unsubscribe(OpenMenu);

        CloseMenu();
    }

    // Opens the screen and pauses the game
    protected virtual void OpenMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Closes the screen, unpauses the game, and notifies other systems
    public virtual void CloseMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (onStationClosed != null)
        {
            onStationClosed.Raise();
        }
    }
}