using System.Collections;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject chestPanel;

    [Header("Event Listeners")]
    [SerializeField] private Vector3IntEvent OnChestUsed;
    [SerializeField] private GameEvent OnChestClosed;

    [SerializeField] private float pauseDelay;

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
        if (OnChestUsed != null)
        {
            OnChestUsed.Subscribe(OpenChestPanel);
        }
    }

    private void OnDisable()
    {
        if (OnChestUsed != null)
        {
            OnChestUsed.Unsubscribe(OpenChestPanel);
        }

        CloseChestPanel();
    }

    private void OpenChestPanel(Vector3Int position)
    {
        StartCoroutine(OpenChestPanel());
    }

    public void CloseChestPanel() // Public so it can be assigned to an 'X' button
    {
        chestPanel.SetActive(false);

        // Resume game time and player input
        Time.timeScale = 1;

        // Announce the closure via event for any other system (like InputManager) to react
        if (OnChestClosed != null)
        {
            OnChestClosed.Raise();
        }
    }

    IEnumerator OpenChestPanel()
    {
        yield return new WaitForSeconds(pauseDelay);
        chestPanel.SetActive(true);
    }
}
