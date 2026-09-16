using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchanterUI : StationUI
{
    [Header("UI Elements")]
    [SerializeField] private InventorySlotUI oreSlot;
    [SerializeField] private InventorySlotUI gemSlot;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button button;
    [SerializeField] private Button diamondButton;
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("References")]
    [SerializeField] private ItemData[] gems;
    

    private BaseStorage Storage => BaseStorage.Current;

    private EnchantData enchant;
    private EnchantSlot selectedSlot;

    private void Start()
    {
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (var gem in gems)
            options.Add(new TMP_Dropdown.OptionData(gem.displayName, gem.icon, Color.white));

        dropdown.AddOptions(options);
    }

    protected override void OpenMenu()
    {
        base.OpenMenu();
        UpdateDisplay(gems[dropdown.value]);
    }

    private void UpdateDisplay(ItemData gem)
    {
        enchant = EnchantDatabase.GetEnchant(selectedSlot, gem);

        oreSlot.SetRequired(enchant.requiredMagicOre, enchant.magicOreAmount);
        gemSlot.SetRequired(gem, 1);

        infoText.text = enchant.description;
        button.interactable = CanEnchant();
        diamondButton.interactable = CanUseDiamond();
    }

    public void DropdownChanged()
    {
        UpdateDisplay(gems[dropdown.value]);
    }

    private bool CanEnchant()
    {
        if (Storage.GetQuantity(enchant.requiredMagicOre) < enchant.magicOreAmount)
            return false;

        if (Storage.GetQuantity(enchant.requiredGem) < 1)
            return false;

        return true;
    }

    private bool CanUseDiamond()
    {
        return !button.interactable && Storage.GetQuantity("diamond") > 0;
    }

    public void NormalEnchant()
    {
        Storage.RemoveItem(enchant.requiredGem, 1);
        Enchant();
    }

    public void UseDiamond()
    {
        Storage.RemoveItem("diamond", 1);
        Enchant();
    }

    private void Enchant()
    {
        Storage.RemoveItem(enchant.requiredMagicOre, enchant.magicOreAmount); // Since requires magic ore either way
        GameSession.Instance.runState.equippedEnchants[selectedSlot] = new ActiveEnchant
        {
            data = enchant,
            runsRemaining = enchant.runsPerEnchant
        };
        UpdateDisplay(enchant.requiredGem);
    }

    public void OnPickaxeTabSelected()
    {
        selectedSlot = EnchantSlot.Pickaxe;
        UpdateDisplay(gems[dropdown.value]);
    }

    public void OnArmourTabSelected()
    {
        selectedSlot = EnchantSlot.Armour;
        UpdateDisplay(gems[dropdown.value]);
    }

    public void OnSwordTabSelected()
    {
        selectedSlot = EnchantSlot.Sword;
        UpdateDisplay(gems[dropdown.value]);
    }
}
