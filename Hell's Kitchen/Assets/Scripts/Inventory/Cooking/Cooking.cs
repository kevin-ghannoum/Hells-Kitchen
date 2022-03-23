using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    private bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList)
    {
        Dictionary<Item, bool> ingredientCheckList = new Dictionary<Item, bool>();
        foreach (var (item, quantity) in ingredientList)
        {
            if (Player.Instance.GetPlayerInventory().ContainsKey(item)) // if ingredient in inventory
            {
                if (Player.Instance.GetPlayerInventory()[item] >= quantity) // if ingredient qt sufficient
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
                Player.Instance.RemoveItemFromInventory(item, quantity);
            }
            
            // add recipe result to inventory
            Player.Instance.AddItemToInventory(recipe.GetRecipeResult(), 1);

            return true;
        }

        return false;
    }
}
