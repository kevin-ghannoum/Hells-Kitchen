using System.Collections.Generic;
using Player;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    private bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList)
    {
        if (PlayerController.Instance.GetPlayerInventory().Count == 0)
            return false;
        
        Dictionary<Item, bool> ingredientCheckList = new Dictionary<Item, bool>();
        foreach (var (item, quantity) in ingredientList)
        {
            if (PlayerController.Instance.GetPlayerInventory().ContainsKey(item)) // if ingredient in inventory
            {
                if (PlayerController.Instance.GetPlayerInventory()[item] >= quantity) // if ingredient qt sufficient
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

    public bool CookRecipe(IRecipe recipe)
    {
        List<(Item item, int quantity)> ingredientList = recipe.GetIngredientList();
        
        if (InventoryContainsAllIngredients(ingredientList))
        {
            // remove used items
            foreach (var (item, quantity) in ingredientList)
            {
                PlayerController.Instance.RemoveItemFromInventory(item, quantity);
            }
            
            // add recipe result to inventory
            PlayerController.Instance.AddItemToInventory(recipe.GetRecipeResult(), 1);
            return true;
        }
        return false;
    }
}
