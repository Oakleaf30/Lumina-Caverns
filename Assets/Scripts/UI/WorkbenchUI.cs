using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PickaxeUpgradeManager;

public class WorkbenchUI : StationUI
{

    [Header("References")]
    [SerializeField] private PlayerMining mining;
    BaseStorage storage => BaseStorage.Instance;

    public PickaxeData pickaxe => mining.pickaxe;
    public PickaxeTier tier => mining.tier;
    public int tierIndex => mining.tierIndex;

    [SerializeField] private PickaxeUpgradeManager pickaxeUpgrade;

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private InventorySlotUI storageSlot;
    [SerializeField] private InventorySlotUI costSlot;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI infoText;

    private ItemData displayItem;

    public void RefreshPickaxeDisplay()
    {
        if (pickaxeUpgrade.NextUpgrade.kind == UpgradeKind.None)
            return;

        int nextTierIndex = tierIndex + 1;
        bool isFlawless = nextTierIndex == pickaxe.tiers.Length - 1
                  && mining.pickaxe != pickaxeUpgrade.upgrades[0];

        PopulateDropdown(isFlawless);

        var nextTier = pickaxeUpgrade.GetNextUpgradeTier();
        displayItem = isFlawless ? GetSelectedGem() : nextTier.costItem;

        infoText.text = $"Durability: {tier.maxDurability} > {nextTier.maxDurability}\n" +
                $"Damage: {tier.damage} > {nextTier.damage}";

        if (isFlawless)
        {
            infoText.text += $"\nNew Ability: {pickaxe.specialAbility.description}";
        }

        storageSlot.Set(displayItem, storage.GetQuantity(displayItem));
        costSlot.Set(displayItem, tier.costAmount);

        button.interactable = pickaxeUpgrade.CanAfford(displayItem, tier);
    }

    private void PopulateDropdown(bool isFlawless)
    {
        if (!isFlawless)
        {
            dropdown.gameObject.SetActive(false);
            return;
        }

        dropdown.gameObject.SetActive(true);
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (var gem in pickaxe.gemOptions)
            options.Add(new TMP_Dropdown.OptionData(gem.displayName, gem.icon, Color.white));

        dropdown.AddOptions(options);
    }

    public ItemData GetSelectedGem()
    {
        return pickaxe.gemOptions[dropdown.value];
    }

    public void DropdownChanged()
    {
        ItemData gem = GetSelectedGem();
        storageSlot.Set(gem, storage.GetQuantity(gem));
        costSlot.Set(gem, tier.costAmount);
    }


    //======================================================================================================================================================
    public enum GearCategory { Pickaxe, Armour, Sword }
    private GearCategory currentCategory = GearCategory.Pickaxe;

    public void OnPickaxeTabSelected()
    {
        currentCategory = GearCategory.Pickaxe;
        RefreshDisplay();
    }

    public void OnArmourTabSelected()
    {
        currentCategory = GearCategory.Armour;
        RefreshDisplay();
    }

    public void OnSwordTabSelected()
    {
        currentCategory = GearCategory.Sword;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        switch (currentCategory)
        {
            case GearCategory.Pickaxe:
                RefreshPickaxeDisplay();
                break;
            case GearCategory.Armour:
                //RefreshArmourDisplay();
                break;
            case GearCategory.Sword:
                //RefreshSwordDisplay();
                break;
        }
    }

    public void CraftUpgrade()
    {
        switch (currentCategory)
        {
            case GearCategory.Pickaxe:
                pickaxeUpgrade.UpgradePickaxe(displayItem);
                RefreshPickaxeDisplay();
                break;
            case GearCategory.Armour:
                //RefreshArmourDisplay();
                break;
            case GearCategory.Sword:
                //RefreshSwordDisplay();
                break;
        }
    }
}