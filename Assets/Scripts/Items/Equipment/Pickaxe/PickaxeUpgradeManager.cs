using UnityEngine;

public class PickaxeUpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerMining mining;
    BaseStorage storage => BaseStorage.Current;

    public PickaxeData[] upgrades;

    public PickaxeData pickaxe => GameSession.Instance.runState.pickaxe;
    public int pickaxeIndex => GameSession.Instance.runState.pickaxeIndex;
    public int tierIndex => GameSession.Instance.runState.tierIndex;

    public enum UpgradeKind { None, TierUpgrade, NewType }

    public struct NextUpgradeInfo
    {
        public UpgradeKind kind;
        public PickaxeTier tier;      // valid only if kind == TierUpgrade
        public PickaxeData newType;   // valid only if kind == NewLine
    }

    public PickaxeTier GetNextUpgradeTier()
    {
        NextUpgradeInfo info = NextUpgrade;

        switch (info.kind)
        {
            case UpgradeKind.TierUpgrade:
                return info.tier;

            case UpgradeKind.NewType:
                return info.newType.tiers[0];

            case UpgradeKind.None:
                return default;
        }

        return default;
    }

    public bool CanAfford(ItemData item, PickaxeTier tier)
    {
        bool test = storage.GetQuantity(item) >= tier.costAmount;
        return test;
    }

    public NextUpgradeInfo NextUpgrade
    {
        get
        {
            int nextTierIndex = tierIndex + 1;
            if (nextTierIndex < pickaxe.tiers.Length)
                return new NextUpgradeInfo { kind = UpgradeKind.TierUpgrade, tier = pickaxe.tiers[nextTierIndex] };

            int nextTypeIndex = pickaxeIndex + 1;
            if (nextTypeIndex < upgrades.Length)
                return new NextUpgradeInfo { kind = UpgradeKind.NewType, newType = upgrades[nextTypeIndex] };

            // fell through both checks — no next tier, no next ore
            return new NextUpgradeInfo { kind = UpgradeKind.None };
        }
    }

    public void UpgradePickaxe(ItemData item)
    {
        var tier = GetNextUpgradeTier();

        UpdatePickaxeStatus();

        storage.RemoveItem(item, tier.costAmount);
    }

    private void UpdatePickaxeStatus()
    {
        NextUpgradeInfo info = NextUpgrade;

        switch (info.kind)
        {
            case UpgradeKind.TierUpgrade:
                GameSession.Instance.runState.tierIndex++;
                GameSession.Instance.runState.tier = pickaxe.tiers[tierIndex];
                break;

            case UpgradeKind.NewType:
                GameSession.Instance.runState.pickaxeIndex++;
                GameSession.Instance.runState.pickaxe = upgrades[pickaxeIndex];
                GameSession.Instance.runState.tier = pickaxe.tiers[0];
                break;
        }
    }
}

