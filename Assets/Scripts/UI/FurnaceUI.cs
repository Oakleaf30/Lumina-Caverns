using TMPro;
using UnityEngine;

public class FurnaceUI : StationUI
{
    BaseStorage storage => BaseStorage.Instance;

    [SerializeField] private InventorySlotUI slot1;
    [SerializeField] private InventorySlotUI slot2;
    [SerializeField] private InventorySlotUI slot3;

    protected override void OpenMenu()
    {
        base.OpenMenu();
    }

    public void SetResources(RecipeData recipe)
    {
        slot1.Set(recipe.ingredients[0].item, storage.GetQuantity(recipe.ingredients[0].item));
        slot2.Set(recipe.ingredients[1].item, storage.GetQuantity(recipe.ingredients[1].item));
        slot3.Set(recipe.resultItem, storage.GetQuantity(recipe.resultItem));
    }
}