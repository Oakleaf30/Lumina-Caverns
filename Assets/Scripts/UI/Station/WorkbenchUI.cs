using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PickaxeUpgradeManager;

public class WorkbenchUI : TabPanelUI
{
    BaseStorage storage => BaseStorage.Current;
    private RunState RunState => GameSession.Instance.runState;

    private PickaxeData pickaxe => RunState.pickaxe;
    private int pickaxeIndex => RunState.pickaxeIndex;
    private PickaxeTier tier => RunState.tier;
    private int tierIndex => RunState.tierIndex;
    private PickaxeData NextPickaxe => pickaxeUpgrade.GetNextPickaxe();

    private ArmourData Armour => RunState.armour;
    private int ArmourIndex => RunState.armourIndex;
    private ArmourData NextArmour => equipmentUpgrade.ReturnNextArmour(ArmourIndex);

    private SwordData Sword => RunState.sword;
    private int SwordIndex => RunState.swordIndex;
    private SwordData NextSword => equipmentUpgrade.ReturnNextSword(SwordIndex);




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

    public override void UpdateDisplay()
    {
        if (pickaxeUpgrade.NextUpgrade.kind == UpgradeKind.None)
            return;

        int nextTierIndex = tierIndex + 1;
        bool isFlawless = nextTierIndex == pickaxe.tiers.Length - 1
                  && pickaxeIndex != 0;

        PopulateDropdown(isFlawless);

        var nextTier = pickaxeUpgrade.GetNextUpgradeTier();
        displayItem = isFlawless ? GetSelectedGem() : nextTier.costItem;

        UpdateInfoText(nextTier);

        RefreshContainers();

        storageSlot.Set(displayItem, storage.GetQuantity(displayItem));
        costSlot.Set(displayItem, nextTier.costAmount);

        button.interactable = pickaxeUpgrade.CanAfford(displayItem, nextTier);
    }

    private void UpdateInfoText(PickaxeTier nextTier)
    {
        if (tierIndex == pickaxe.tiers.Length - 1)
        {
            infoText.text = $"Durability: {tier.maxDurability} > {nextTier.maxDurability}\n" +
                $"Damage: {pickaxe.damage} > {NextPickaxe.damage}";
        } else
        {
            switch (tierIndex)
            {
                case 0:
                    infoText.text = $"Durability: {tier.maxDurability} > {nextTier.maxDurability}\n" +
                    $"Durability repaired per bar: {RunState.durabilityPerBar} > {pickaxe.durabilityPerBar}";
                    break;
                case 1:
                    infoText.text = $"Durability: {tier.maxDurability} > {nextTier.maxDurability}\n" +
                    $"New Ability: {pickaxe.specialAbility.description}";
                    break;
            }
        }
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

    private void RefreshContainers()
    {
        int nextTierIndex = tierIndex + 1 >= pickaxe.tiers.Length ? 0 : tierIndex + 1;
        currentSlotP.Set(pickaxe, tierIndex, SlotDisplayMode.Equipment);
        nextSlotP.Set(NextPickaxe, nextTierIndex, SlotDisplayMode.Equipment);

        currentSlotA.Set(Armour, 0, SlotDisplayMode.Equipment);
        nextSlotA.Set(NextArmour, 0, SlotDisplayMode.Equipment);

        currentSlotS.Set(Sword, 0, SlotDisplayMode.Equipment);
        nextSlotS.Set(NextSword, 0, SlotDisplayMode.Equipment);
    }

    private void RefreshArmourDisplay()
    {
        infoText.text = $"Health Increaase: {Armour.maxHealth} > {NextArmour.maxHealth}";

        RefreshContainers();

        var costItem = NextArmour.costItem;
        storageSlot.Set(costItem, storage.GetQuantity(costItem));
        costSlot.Set(costItem, NextArmour.costAmount);

        button.interactable = equipmentUpgrade.CanAfford(costItem, NextArmour);
    }

    private void RefreshSwordDisplay()
    {
        infoText.text = $"Damage Increaase: {Sword.damage} > {NextSword.damage}";

        RefreshContainers();

        var costItem = NextSword.costItem;
        storageSlot.Set(costItem, storage.GetQuantity(costItem));
        costSlot.Set(costItem, NextSword.costAmount);

        button.interactable = equipmentUpgrade.CanAfford(costItem, NextSword);
    }


    //======================================================================================================================================================


    private enum GearCategory { Pickaxe, Armour, Sword }
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
                UpdateDisplay();
                break;
            case GearCategory.Armour:
                RefreshArmourDisplay();
                break;
            case GearCategory.Sword:
                RefreshSwordDisplay();
                break;
        }
    }

    public void CraftUpgrade()
    {
        switch (currentCategory)
        {
            case GearCategory.Pickaxe:
                pickaxeUpgrade.UpgradePickaxe(displayItem);
                UpdateDisplay();
                break;
            case GearCategory.Armour:
                equipmentUpgrade.UpgradeArmour(NextArmour);
                RefreshArmourDisplay();
                break;
            case GearCategory.Sword:
                equipmentUpgrade.UpgradeSword(NextSword);
                RefreshSwordDisplay();
                break;
        }
    }
}