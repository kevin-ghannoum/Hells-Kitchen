using System;
using System.Collections.Generic;
using Common;
using Enums.Items;

namespace PlayerInventory.Cooking
{
    public static class Cooking
    {
        private static bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList, int count = 1)
        {
            var inventory = GameStateManager.Instance.GetPlayerInventory();
            if (inventory.GetInventoryItems().Count == 0)
                return false;
            
            foreach (var (item, quantity) in ingredientList)
            {
                if (!inventory.GetInventoryItems().ContainsKey(item.ItemInstance) || inventory.GetInventoryItems()[item.ItemInstance] < quantity * count )
                    return false;
            }
            
            return true;
        }

        public static bool CookRecipe(IRecipe recipe, int recipeCount = 1)
        {
            List<(Item item, int quantity)> ingredientList = recipe.GetIngredientList();
        
            if (InventoryContainsAllIngredients(ingredientList, recipeCount))
            {
                // remove used items
                foreach (var (item, quantity) in ingredientList)
                {
                    GameStateManager.RemoveItemFromInventory(item.ItemInstance, quantity * recipeCount);
                }
            
                // add recipe result to inventory
                GameStateManager.AddItemToInventory(recipe.GetRecipeResult().ItemInstance, recipeCount);
                return true;
            }
            return false;
        }
        
        public static IRecipe GetItemRecipe(ItemInstance item)
        {
            return (IRecipe)Activator.CreateInstance(Type.GetType($"PlayerInventory.Cooking.Recipes+{item}"));
        }
    }
}
