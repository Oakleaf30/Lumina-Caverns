using UnityEngine;
using UnityEngine.UI;

public class CauldronUI : StationUI
{
    [Header("UI References")]
    [SerializeField] private InventorySlotUI slot1;
    [SerializeField] private InventorySlotUI slot2;
    [SerializeField] private Button button;

    [Header("References")]
    [SerializeField] private RecipeData recipe;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private GameEvent onPotionCountChanged;

    private BaseStorage Storage => BaseStorage.Current;

    override protected void OpenMenu()
    {
        base.OpenMenu();

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        slot1.Set(recipe.ingredients[0].item, inventory.GetQuantity(recipe.ingredients[0].item));
        slot1.quantityText.text += "/5";

        slot2.Set(recipe.resultItem, Storage.GetQuantity(recipe.resultItem));

        button.interactable = inventory.GetQuantity(recipe.ingredients[0].item) >= 5;
    }

    public void Craft()
    {
        inventory.RemoveItem(recipe.ingredients[0].item, recipe.ingredients[0].requiredAmount);
        Storage.AddItem(recipe.resultItem, 1);
        onPotionCountChanged.Raise();

        UpdateDisplay();
    }
}
