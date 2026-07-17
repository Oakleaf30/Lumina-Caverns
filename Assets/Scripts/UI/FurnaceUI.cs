using UnityEngine;
using UnityEngine.UI;

public class FurnaceUI : StationUI
{
    BaseStorage storage => BaseStorage.Instance;

    [Header("Resource Section")]
    [SerializeField] private InventorySlotUI slotR1;
    [SerializeField] private InventorySlotUI slotR2;
    [SerializeField] private InventorySlotUI slotR3;

    [Header("Smelting Section")]
    [SerializeField] private InventorySlotUI slotF1;
    [SerializeField] private InventorySlotUI slotF2;
    [SerializeField] private InventorySlotUI slotF3;

    [Space(10)]
    [SerializeField] private Slider slider;
    [SerializeField] private Button button;

    private RecipeData currentRecipe;
    private int quantity;

    protected override void OpenMenu()
    {
        base.OpenMenu();
    }

    public void SetResources(RecipeData recipe)
    {
        slotR1.Set(recipe.ingredients[0].item, storage.GetQuantity(recipe.ingredients[0].item));
        slotR2.Set(recipe.ingredients[1].item, storage.GetQuantity(recipe.ingredients[1].item));
        slotR3.Set(recipe.resultItem, storage.GetQuantity(recipe.resultItem));

        slider.maxValue = Mathf.Min(
            storage.GetQuantity(recipe.ingredients[0].item) / recipe.ingredients[0].requiredAmount,
            storage.GetQuantity(recipe.ingredients[1].item) / recipe.ingredients[1].requiredAmount
            );
        slider.value = 0;
        slider.interactable = true;

        slotF1.Set(recipe.ingredients[0].item, 0);
        slotF2.Set(recipe.ingredients[1].item, 0);
        slotF3.Set(recipe.resultItem, 0);

        currentRecipe = recipe;
    }

    public void UpdateSlider()
    {
        quantity = Mathf.RoundToInt(slider.value);

        slotF1.Set(currentRecipe.ingredients[0].item, currentRecipe.ingredients[0].requiredAmount * quantity);
        slotF2.Set(currentRecipe.ingredients[1].item, currentRecipe.ingredients[1].requiredAmount * quantity);
        slotF3.Set(currentRecipe.resultItem, quantity);

        button.interactable = slider.value == 0 ? false : true;
    }

    public void Craft()
    {
        storage.RemoveItem(currentRecipe.ingredients[0].item, currentRecipe.ingredients[0].requiredAmount * quantity);
        storage.RemoveItem(currentRecipe.ingredients[1].item, currentRecipe.ingredients[1].requiredAmount * quantity);
        storage.AddItem(currentRecipe.resultItem, quantity);

        SetResources(currentRecipe);
        UpdateSlider();
    }
}