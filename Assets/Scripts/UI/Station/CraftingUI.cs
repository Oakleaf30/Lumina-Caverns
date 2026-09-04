using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : TabPanelUI
{
    private BaseStorage Storage => BaseStorage.Current;

    [Header("UI References")]
    [SerializeField] private InventorySlotUI slot1;
    [SerializeField] private InventorySlotUI slot2;
    [SerializeField] private InventorySlotUI slot3;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Event References")]
    [SerializeField] private GameEvent onPotionCountChanged;
    [SerializeField] private GameEvent onBombCountChanged;

    private RecipeData currentRecipe;
    private int itemCraftLimit;

    public override void UpdateDisplay() {} // SetResources() is the replacement in this UI

    private void SetResources(RecipeData recipe)
    {
        slot1.Set(recipe.ingredients[0].item, Storage.GetQuantity(recipe.ingredients[0].item));

        if (recipe.ingredients.Length > 1)
            slot2.Set(recipe.ingredients[1].item, Storage.GetQuantity(recipe.ingredients[1].item));
        else
            slot2.Clear();

        slot3.Set(recipe.resultItem, Storage.GetQuantity(recipe.resultItem));

        currentRecipe = recipe;
        SetItemLimit();
        UpdateText();

        button.interactable = CanCraft();
    }

    private void SetItemLimit()
    {
        switch (currentItem)
        {
            case SelectedItem.Potion:
                itemCraftLimit = 5;
                break;
            case SelectedItem.Bomb:
                itemCraftLimit = 3;
                break;
            case SelectedItem.Amulet:
                itemCraftLimit = 1;
                break;
        }
    }

    private void UpdateText()
    {
        switch (currentItem)
        {
            case SelectedItem.Potion:
                infoText.text = "Restores 30% max health over 10 seconds\n" +
                                "Max carry capacity of 5\n" +
                                "Press 1 to drink";
                break;
            case SelectedItem.Bomb:
                infoText.text = "Destroys nodes in a small radius\n" +
                                "Max carry capacity of 3\n" +
                                "Press 2 to drink";
                break;
            case SelectedItem.Amulet:
                infoText.text = "Prevents you from losing items when you die\n" +
                                "Breaks upon use\n";
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


    private enum SelectedItem { Potion, Bomb, Amulet }
    private SelectedItem currentItem = SelectedItem.Potion;

    public void OnPotionTabSelected(RecipeData recipe)
    {
        currentItem = SelectedItem.Potion;
        SetResources(recipe);
    }

    public void OnBombTabSelected(RecipeData recipe)
    {
        currentItem = SelectedItem.Bomb;
        SetResources(recipe);
    }

    public void OnAmuletTabSelected(RecipeData recipe)
    {
        currentItem = SelectedItem.Amulet;
        SetResources(recipe);
    }
}