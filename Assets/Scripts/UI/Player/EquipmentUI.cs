using TMPro;
using UnityEngine;

public class EquipmentUI : TabPanelUI
{
    private RunState RunState => GameSession.Instance.runState;

    [Header("Equipment Section")]
    [SerializeField] private InventorySlotUI slotE1;
    [SerializeField] private InventorySlotUI slotE2;
    [SerializeField] private InventorySlotUI slotE3;

    [Header("Item Section")]
    [SerializeField] private TextMeshProUGUI slotI1;
    [SerializeField] private TextMeshProUGUI slotI2;
    [SerializeField] private InventorySlotUI slotI3;

    public override void UpdateDisplay()
    {
        slotE1.Set(RunState.pickaxe, RunState.tierIndex, SlotDisplayMode.Equipment);
        slotE2.Set(RunState.armour, 0, SlotDisplayMode.Equipment);
        slotE3.Set(RunState.sword, 0, SlotDisplayMode.Equipment);

        slotI1.text = RunState.potionCount.ToString();
        slotI2.text = RunState.bombCount.ToString();
        slotI3.icon.color = RunState.amuletActive ? Color.white : Color.black;
    }
}
