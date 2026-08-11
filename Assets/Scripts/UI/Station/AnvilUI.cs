using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnvilUI : TabPanelUI
{
    BaseStorage storage => BaseStorage.Current;
    
    private int durabilityRepaired;
    private int cost;

    [Header("Settings")]
    [SerializeField] int durabilityPerBar;

    [Header("UI References")]
    [SerializeField] InventorySlotUI display;
    [SerializeField] private TextMeshProUGUI preview;
    [SerializeField] InventorySlotUI costSlot;
    [SerializeField] Button normal;
    [SerializeField] Button emergency;

    [Header("References")]
    [SerializeField] private PlayerMining mining;
    [SerializeField] ItemData bar;


    public override void UpdateDisplay()
    {
        display.Set(mining.pickaxe, mining.tierIndex, SlotDisplayMode.Equipment);

        durabilityPerBar = GameSession.Instance.runState.tierIndex == 0 ? 20 : 30;

        durabilityRepaired = CalculateCost();
        preview.text = $"{mining.pickaxeDurability}/{mining.maxPickaxeDurability} > {mining.pickaxeDurability + durabilityRepaired}/{mining.maxPickaxeDurability}";

        UpdateButtons();
    }

    private int CalculateCost()
    {
        int neededDurability = mining.maxPickaxeDurability - mining.pickaxeDurability;
        cost = Mathf.CeilToInt((float)neededDurability / durabilityPerBar);

        costSlot.Set(bar, Mathf.Min(storage.GetQuantity(bar), cost));

        if (storage.GetQuantity(bar) < cost)
        {
            return storage.GetQuantity(bar) * durabilityPerBar;
        } else
        {
            return neededDurability;
        }
    }

    private void UpdateButtons()
    {
        normal.interactable = storage.GetQuantity(bar) > 0 && mining.pickaxeDurability != mining.maxPickaxeDurability;
        emergency.interactable = storage.GetQuantity(bar) == 0 && mining.pickaxeDurability < 10;
    }

    public void NormalRepair()
    {
        mining.pickaxeDurability += durabilityRepaired;
        storage.RemoveItem(bar, Mathf.Min(storage.GetQuantity(bar), cost));
        UpdateDisplay();
    }

    public void EmergencyRepair()
    {
        mining.pickaxeDurability = 10;
        UpdateDisplay();
    }
}