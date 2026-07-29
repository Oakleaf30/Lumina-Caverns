using UnityEngine;
using System.Linq;

public class PlayerInventory : ItemContainer
{
    [SerializeField] private GameEvent onInventoryOpen;
    [SerializeField] private GameEvent onReturnBase;

    private void Start()
    {
        items = GameSession.Instance.runState.inventory;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            onInventoryOpen.Raise();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            onReturnBase.Raise();
            DepositAllTo(BaseStorage.Current);
            GameSession.Instance.ExitMines();
        }
    }

    private void DepositAllTo(BaseStorage baseStorage)
    {
        foreach (var kv in items)
            baseStorage.AddItem(kv.Key, kv.Value);

        items.Clear();
    }

    public void ApplyDeathPenalty(bool hasAmuletProtection)
    {
        var toRemove = items.Keys
            .Where(i => i.category == ItemCategory.Gem || i.category == ItemCategory.Ore)
            .ToList();

        if (hasAmuletProtection)
            toRemove = toRemove.Where(i => i.category != ItemCategory.Gem).ToList();

        foreach (var item in toRemove) items.Remove(item);
    }
}