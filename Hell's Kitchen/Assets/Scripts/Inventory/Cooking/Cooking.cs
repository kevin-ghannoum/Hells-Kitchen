using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    private bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList)
    {
        Character player = Character.Instance;
        Dictionary<Item, bool> ingredientCheckList = new Dictionary<Item, bool>();
        foreach (var (item, quantity) in ingredientList)
        {
            if (player.GetPlayerInventory().ContainsKey(item)) // if ingredient in inventory
            {
                if (player.GetPlayerInventory()[item] >= quantity) // if ingredient qt sufficient
                {
                    ingredientCheckList[item] = true;
                }
                else
                {
                    ingredientCheckList[item] = false;
                }
            }
        }

        foreach (var isQtSufficient in ingredientCheckList.Values)
        {
            if (!isQtSufficient)
            {
                return false;
            }
        }                
        
        return true;
    }

    private bool CookRecipe(IRecipe recipe)
    {
        List<(Item item, int quantity)> ingredientList = recipe.GetIngredientList();
        
        if (InventoryContainsAllIngredients(ingredientList))
        {
            // remove used items
            foreach (var (item, quantity) in ingredientList)
            {
                Character.Instance.RemoveItemFromInventory(item, quantity);
            }
            
            // add recipe result to inventory
            Character.Instance.AddItemToInventory(recipe.GetRecipeResult(), 1);

            return true;
        }

        return false;
    }
}
