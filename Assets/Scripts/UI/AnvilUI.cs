using UnityEngine;

public class AnvilUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject anvilPanel;

    [Header("Event Listeners")]
    [SerializeField] private GameEvent OnAnvilUsed;
    [SerializeField] private GameEvent OnAnvilClosed;

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
        if (OnAnvilUsed != null)
        {
            OnAnvilUsed.Subscribe(OpenAnvilPanel);
        }
    }

    private void OnDisable()
    {
        if (OnAnvilUsed != null)
        {
            OnAnvilUsed.Unsubscribe(OpenAnvilPanel);
        }

        CloseAnvilPanel();
    }

    private void OpenAnvilPanel()
    {
        // Set the panel active
        anvilPanel.SetActive(true);

        // Unlock the mouse cursor (if a PC game) and pause player input
        Time.timeScale = 0; // Example: Pause the game time
    }

    public void CloseAnvilPanel() // Public so it can be assigned to an 'X' button
    {
        anvilPanel.SetActive(false);

        // Resume game time and player input
        Time.timeScale = 1;

        // Announce the closure via event for any other system (like InputManager) to react
        if (OnAnvilClosed != null)
        {
            OnAnvilClosed.Raise();
        }
    }
}
