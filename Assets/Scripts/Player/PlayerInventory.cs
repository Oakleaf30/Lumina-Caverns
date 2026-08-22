using UnityEngine;
using System.Linq;

public class PlayerInventory : ItemContainer
{
    [SerializeField] private GameEvent onInventoryOpen;
    [SerializeField] private GameEvent onReturnBase;
    [SerializeField] private GameEvent onPlayerDeath;
    [SerializeField] private ItemData amulet;

    private RunState RunState => GameSession.Instance.runState;
    private BaseStorage Storage => BaseStorage.Current;


    private void Start()
    {
        items = RunState.inventory;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            onInventoryOpen.Raise();
        }

        if (Input.GetKeyDown(KeyCode.B) && RunState.currentHealth > 0)
        {
            ReturnToBase();
        }
    }

    private void ReturnToBase()
    {
        onReturnBase.Raise();
        DepositItems();
        GameSession.Instance.ExitMines();
    }

    private void DepositItems()
    {
        foreach (var kv in items)
            Storage.AddItem(kv.Key, kv.Value);

        items.Clear();
    }

    private void OnEnable()
    {
        onPlayerDeath.Subscribe(ApplyDeathPenalty);
    }

    private void OnDisable()
    {
        onPlayerDeath.Unsubscribe(ApplyDeathPenalty);
    }

    public void ApplyDeathPenalty()
    {
        if (RunState.amuletActive)
            Storage.RemoveItem(amulet, 1);
        else
            items.Clear();

        ReturnToBase();
    }
}