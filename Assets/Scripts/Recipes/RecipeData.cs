using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeData", menuName = "Lumina Caverns/Recipe Data")]
public class RecipeData : ScriptableObject
{
    public ItemData resultItem;
    public RecipeIngredient[] ingredients;
}

[System.Serializable]
public struct RecipeIngredient
{
    public ItemData item;
    public int requiredAmount;
}