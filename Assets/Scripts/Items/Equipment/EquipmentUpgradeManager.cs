using UnityEngine;

public class EquipmentUpgradeManager : MonoBehaviour
{
    [SerializeField] private EquipmentRegistry registry;

    BaseStorage storage => BaseStorage.Current;
    RunState RunState => GameSession.Instance.runState;

    public ArmourData ReturnNextArmour(int armourIndex)
    {
        int nextArmourIndex = armourIndex + 1;
        return nextArmourIndex < registry.armour.Length ? registry.armour[nextArmourIndex] : null;
    }

    public void UpgradeArmour(ArmourData armour)
    {
        RunState.armourIndex++;
        RunState.armour = registry.armour[RunState.armourIndex];

        storage.RemoveItem(armour.costItem, armour.costAmount);
    }

    public bool CanAfford(ItemData item, EquipmentData equipment)
    {
        bool test = storage.GetQuantity(item) >= equipment.costAmount;
        return test;
    }

    public SwordData ReturnNextSword(int swordIndex)
    {
        int nextSwordIndex = swordIndex + 1;
        return nextSwordIndex < registry.swords.Length ? registry.swords[nextSwordIndex] : null;
    }

    public void UpgradeSword(SwordData sword)
    {
        RunState.swordIndex++;
        RunState.sword = registry.swords[RunState.swordIndex];

        storage.RemoveItem(sword.costItem, sword.costAmount);
    }
}
