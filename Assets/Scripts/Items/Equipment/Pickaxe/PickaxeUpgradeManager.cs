using UnityEngine;

public class PickaxeUpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerMining mining;
    BaseStorage storage => BaseStorage.Instance;

    [SerializeField] private PickaxeData[] pickaxeUpgrades;

    public PickaxeData pickaxe => mining.pickaxe;
    public int pickaxeIndex => mining.pickaxeIndex;
    public PickaxeTier tier => mining.tier;
    public int tierIndex => mining.tierIndex;

    public enum UpgradeKind { None, TierUpgrade, NewType }

    public struct NextUpgradeInfo
    {
        public UpgradeKind kind;
        public PickaxeTier tier;      // valid only if kind == TierUpgrade
        public PickaxeData newType;   // valid only if kind == NewLine
    }

    public bool CanAfford()
    {
        NextUpgradeInfo info = NextUpgrade;

        switch (info.kind)
        {
            case UpgradeKind.TierUpgrade:
                if (storage.GetQuantity(info.tier.costItem) >= info.tier.costAmount)
                    return true;

                break;

            case UpgradeKind.NewType:
                if (storage.GetQuantity(info.newType.tiers[0].costItem) >= info.newType.tiers[0].costAmount)
                    return true;
                break;

            case UpgradeKind.None:
                return false;
        }

        return false;
    }

    private NextUpgradeInfo NextUpgrade
    {
        get
        {
            int nextTierIndex = tierIndex + 1;
            if (nextTierIndex < pickaxe.tiers.Length)
                return new NextUpgradeInfo { kind = UpgradeKind.TierUpgrade, tier = pickaxe.tiers[nextTierIndex] };

            int nextTypeIndex = pickaxeIndex + 1;
            if (nextTypeIndex < pickaxeUpgrades.Length)
                return new NextUpgradeInfo { kind = UpgradeKind.NewType, newType = pickaxeUpgrades[nextTypeIndex] };

            // fell through both checks — no next tier, no next ore
            return new NextUpgradeInfo { kind = UpgradeKind.None };
        }
    }

}

