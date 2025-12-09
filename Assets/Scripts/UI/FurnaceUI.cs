using UnityEngine;

public class FurnaceUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject furnacePanel;

    [Header("Event Listeners")]
    [SerializeField] private GameEvent OnFurnaceUsed;
    [SerializeField] private GameEvent OnFurnaceClosed;

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
        if (OnFurnaceUsed != null)
        {
            OnFurnaceUsed.Subscribe(OpenFurnacePanel);
        }
    }

    private void OnDisable()
    {
        if (OnFurnaceUsed != null)
        {
            OnFurnaceUsed.Unsubscribe(OpenFurnacePanel);
        }

        CloseFurnacePanel();
    }

    private void OpenFurnacePanel()
    {
        // Set the panel active
        furnacePanel.SetActive(true);

        // Unlock the mouse cursor (if a PC game) and pause player input
        Time.timeScale = 0; // Example: Pause the game time
    }

    public void CloseFurnacePanel() // Public so it can be assigned to an 'X' button
    {
        furnacePanel.SetActive(false);

        // Resume game time and player input
        Time.timeScale = 1;

        // Announce the closure via event for any other system (like InputManager) to react
        if (OnFurnaceClosed != null)
        {
            OnFurnaceClosed.Raise();
        }
    }
}
