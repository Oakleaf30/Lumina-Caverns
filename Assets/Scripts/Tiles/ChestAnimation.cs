using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class ChestAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AnimatedEventTile chestTile;
    [SerializeField] private Tilemap interactionTilemap;

    [Header("Event Listeners")]
    [SerializeField] private Vector3IntEvent OnChestUsed;
    [SerializeField] private GameEvent OnChestClosed;

    [SerializeField] private float chestOpenDelay;
    [SerializeField] private float chestCloseDelay;

    private Vector3Int currentChestPosition;

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
            OnChestUsed.Subscribe(OpenChest);
        }
    }

    private void OnDisable()
    {
        if (OnChestUsed != null)
        {
            OnChestUsed.Unsubscribe(OpenChest);
        }
    }

    private void OpenChest(Vector3Int position)
    {
        currentChestPosition = position;
        chestTile.SetAnimationState(position, interactionTilemap, false);
        StartCoroutine(PauseGameTime());
    }

    public void CloseChest() // Public so it can be assigned to an 'X' button
    {
        StartCoroutine(FinishChestAnimation());
    }

    IEnumerator PauseGameTime()
    {
        yield return new WaitForSeconds(chestOpenDelay);
        // Set the panel active

        // Unlock the mouse cursor (if a PC game) and pause player input
        Time.timeScale = 0; // Example: Pause the game time
    }

    IEnumerator FinishChestAnimation()
    {
        yield return new WaitForSeconds(chestCloseDelay);

        chestTile.SetAnimationState(currentChestPosition, interactionTilemap, true);
        currentChestPosition = Vector3Int.zero;
    }
}
