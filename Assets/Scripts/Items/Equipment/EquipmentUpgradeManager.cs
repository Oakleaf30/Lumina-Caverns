using UnityEngine;

public class EquipmentUpgradeManager : MonoBehaviour
{
    [SerializeField] private EquipmentRegistry registry;

    BaseStorage storage => BaseStorage.Current;
    RunState runState => GameSession.Instance.runState;

    public ArmourData ReturnNextArmour(int armourIndex)
    {
        int nextArmourIndex = armourIndex + 1;
        return nextArmourIndex < registry.armour.Length ? registry.armour[nextArmourIndex] : null;
    }

    public void UpgradeArmour(ArmourData armour)
    {
        runState.armourIndex++;
        runState.armour = registry.armour[runState.armourIndex];

        storage.RemoveItem(armour.costItem, armour.costAmount);
    }

    public bool CanAfford(ItemData item, EquipmentData equipment)
    {
        bool test = storage.GetQuantity(item) >= equipment.costAmount;
        return test;
    }


}
