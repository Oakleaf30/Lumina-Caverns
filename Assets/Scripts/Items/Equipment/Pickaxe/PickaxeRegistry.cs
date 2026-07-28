using UnityEngine;

public class PickaxeRegistry : ScriptableObject
{
    public PickaxeData[] upgrades;

    public PickaxeData GetByTier(int tierIndex)
    {
        return upgrades[tierIndex];
    }
}