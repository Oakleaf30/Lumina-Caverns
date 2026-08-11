using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : TabPanelUI
{
    BaseStorage Storage => BaseStorage.Current;

    [Header("UI References")]
    [SerializeField] private InventorySlotUI slot1;
    [SerializeField] private InventorySlotUI slot2;
    [SerializeField] private InventorySlotUI slot3;

    [SerializeField] private Button button;

    private RecipeData currentRecipe;
    private int itemCraftLimit;

    public override void UpdateDisplay()
    {
        // SetResources() is the replacement in this UI
    }

    public void SetResources(RecipeData recipe)
    {
        slot1.Set(recipe.ingredients[0].item, Storage.GetQuantity(recipe.ingredients[0].item));

        if (recipe.ingredients.Length > 1)
            slot2.Set(recipe.ingredients[1].item, Storage.GetQuantity(recipe.ingredients[1].item));
        else
            slot2.Clear();

        slot3.Set(recipe.resultItem, Storage.GetQuantity(recipe.resultItem));

        currentRecipe = recipe;
        SetItemLimit();

        button.interactable = CanCraft();
    }

    private void SetItemLimit()
    {
        switch (currentRecipe.resultItem.itemId)
        {
            case "potion":
                itemCraftLimit = 5;
                break;
            case "bomb":
                itemCraftLimit = 3;
                break;
            case "amulet":
                itemCraftLimit = 1;
                break;

        }
    }

    private bool CanCraft()
    {
        if (Storage.GetQuantity(currentRecipe.resultItem) >= itemCraftLimit)
            return false;

        foreach (var ingredient in currentRecipe.ingredients)
        {
            if (Storage.GetQuantity(ingredient.item) < ingredient.requiredAmount)
                return false;
        }
        return true;
    }

    public void Craft()
    {
        Storage.RemoveItem(currentRecipe.ingredients[0].item, currentRecipe.ingredients[0].requiredAmount);
        if (currentRecipe.ingredients.Length > 1)
            Storage.RemoveItem(currentRecipe.ingredients[1].item, currentRecipe.ingredients[1].requiredAmount);
        Storage.AddItem(currentRecipe.resultItem, 1);

        SetResources(currentRecipe);
    }
}