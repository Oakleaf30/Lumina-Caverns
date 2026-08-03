using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PickaxeUpgradeManager;

public class WorkbenchUI : StationUI
{
    BaseStorage storage => BaseStorage.Current;

    private PickaxeData pickaxe => GameSession.Instance.runState.pickaxe;
    private int pickaxeIndex => GameSession.Instance.runState.pickaxeIndex;
    private PickaxeTier tier => GameSession.Instance.runState.tier;
    private int tierIndex => GameSession.Instance.runState.tierIndex;

    private ArmourData armour => GameSession.Instance.runState.armour;
    private int armourIndex => GameSession.Instance.runState.armourIndex;
    private ArmourData NextArmour => equipmentUpgrade.ReturnNextArmour(armourIndex);




    [SerializeField] private PickaxeUpgradeManager pickaxeUpgrade;
    [SerializeField] private EquipmentUpgradeManager equipmentUpgrade;

    [Header("General UI Elements")]
    [SerializeField] private InventorySlotUI storageSlot;
    [SerializeField] private InventorySlotUI costSlot;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Pickaxe UI Elements")]
    [SerializeField] private InventorySlotUI currentSlotP;
    [SerializeField] private InventorySlotUI nextSlotP;
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("Armour UI Elements")]
    [SerializeField] private InventorySlotUI currentSlotA;
    [SerializeField] private InventorySlotUI nextSlotA;

    [Header("Sword UI Elements")]
    [SerializeField] private InventorySlotUI currentSlotS;
    [SerializeField] private InventorySlotUI nextSlotS;

    private ItemData displayItem;

    public void RefreshPickaxeDisplay()
    {
        if (pickaxeUpgrade.NextUpgrade.kind == UpgradeKind.None)
            return;

        int nextTierIndex = tierIndex + 1;
        bool isFlawless = nextTierIndex == pickaxe.tiers.Length - 1
                  && pickaxeIndex != 0;

        PopulateDropdown(isFlawless);

        var nextTier = pickaxeUpgrade.GetNextUpgradeTier();
        displayItem = isFlawless ? GetSelectedGem() : nextTier.costItem;

        infoText.text = $"Durability: {tier.maxDurability} > {nextTier.maxDurability}\n" +
                $"Damage: {tier.damage} > {nextTier.damage}";

        if (isFlawless)
        {
            infoText.text += $"\nNew Ability: {pickaxe.specialAbility.description}";
        }

        RefreshContainers();

        storageSlot.Set(displayItem, storage.GetQuantity(displayItem));
        costSlot.Set(displayItem, tier.costAmount);

        button.interactable = pickaxeUpgrade.CanAfford(displayItem, nextTier);
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
        var nextTier = pickaxeUpgrade.GetNextUpgradeTier();
        ItemData gem = GetSelectedGem();
        button.interactable = pickaxeUpgrade.CanAfford(gem, nextTier);
        storageSlot.Set(gem, storage.GetQuantity(gem));
        costSlot.Set(gem, tier.costAmount);
    }

    protected override void OpenMenu()
    {
        base.OpenMenu();

        RefreshContainers();
    }

    private void RefreshContainers()
    {
        var nextPickaxe = pickaxeUpgrade.GetNextPickaxe();
        int nextTierIndex = tierIndex + 1 >= pickaxe.tiers.Length ? 0 : tierIndex + 1;
        currentSlotP.Set(pickaxe, tierIndex);
        nextSlotP.Set(nextPickaxe, nextTierIndex);

        currentSlotA.Set(armour, 0);
        nextSlotA.Set(NextArmour, 0);
    }

    public void RefreshArmourDisplay()
    {
        infoText.text = $"Health Increaase: {armour.maxHealth} > {NextArmour.maxHealth}";

        RefreshContainers();

        var costItem = NextArmour.costItem;
        storageSlot.Set(costItem, storage.GetQuantity(costItem));
        costSlot.Set(costItem, NextArmour.costAmount);

        button.interactable = equipmentUpgrade.CanAfford(costItem, NextArmour);
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
                RefreshArmourDisplay();
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
                equipmentUpgrade.UpgradeArmour(NextArmour);
                RefreshArmourDisplay();
                break;
            case GearCategory.Sword:
                //RefreshSwordDisplay();
                break;
        }
    }
}