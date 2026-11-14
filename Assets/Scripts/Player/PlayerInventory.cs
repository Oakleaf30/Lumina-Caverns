using UnityEngine;

public class PlayerInventory : ItemContainer
{
    [SerializeField] private GameEvent onInventoryOpen;
    [SerializeField] private GameEvent onReturnBase;
    [SerializeField] private GameEvent onPlayerDeath;
    [SerializeField] private ItemData amulet;
    [SerializeField] private GameEvent onUIOpen;
    [SerializeField] private ItemEvent onItemCollected;

    private RunState RunState => GameSession.Instance.runState;
    private BaseStorage Storage => BaseStorage.Current;


    private void OnEnable()
    {
        onReturnBase.Subscribe(ReturnToBase);
        onPlayerDeath.Subscribe(ApplyDeathPenalty);
    }

    private void OnDisable()
    {
        onReturnBase.Unsubscribe(ReturnToBase);
        onPlayerDeath.Unsubscribe(ApplyDeathPenalty);
    }

    private void Start()
    {
        items = RunState.inventory;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            onInventoryOpen.Raise();
            onUIOpen.Raise();
        }
    }

    private void ReturnToBase()
    {
        DepositItems();
        GameSession.Instance.ExitMines();
    }

    public override void AddItem(ItemData item, int amount)
    {
        base.AddItem(item, amount);

        onItemCollected.Raise(item);
    }

    private void DepositItems()
    {
        foreach (var kv in items)
            Storage.AddItem(kv.Key, kv.Value);

        items.Clear();
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