using UnityEngine;

public class WorkbenchUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject workbenchPanel;

    [Header("Event Listeners")]
    [SerializeField] private GameEvent OnWorkbenchUsed;
    [SerializeField] private GameEvent OnWorkbenchClosed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        if (OnWorkbenchUsed != null)
        {
            OnWorkbenchUsed.Subscribe(OpenWorkbenchPanel);
        }
    }

    private void OnDisable()
    {
        if (OnWorkbenchUsed != null)
        {
            OnWorkbenchUsed.Unsubscribe(OpenWorkbenchPanel);
        }

        CloseWorkbenchPanel();
    }

    private void OpenWorkbenchPanel()
    {
        // Set the panel active
        workbenchPanel.SetActive(true);

        // Unlock the mouse cursor (if a PC game) and pause player input
        Time.timeScale = 0; // Example: Pause the game time
    }

    public void CloseWorkbenchPanel() // Public so it can be assigned to an 'X' button
    {
        workbenchPanel.SetActive(false);

        // Resume game time and player input
        Time.timeScale = 1;

        // Announce the closure via event for any other system (like InputManager) to react
        if (OnWorkbenchClosed != null)
        {
            OnWorkbenchClosed.Raise();
        }
    }
}
