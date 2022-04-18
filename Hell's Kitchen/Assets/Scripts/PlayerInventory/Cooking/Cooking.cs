using System.Collections.Generic;
using Common;
using Player;

namespace PlayerInventory.Cooking
{
    public static class Cooking
    {
        private static bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList)
        {
            var inventory = GameStateManager.Instance.GetPlayerInventory();
            if (inventory.GetInventoryItems().Count == 0)
                return false;
        
            Dictionary<Item, bool> ingredientCheckList = new Dictionary<Item, bool>();
            
            foreach (var (item, quantity) in ingredientList)
            {
                if (inventory.GetInventoryItems().ContainsKey(item)) // if ingredient in inventory
                {
                    if (inventory.GetInventoryItems()[item] >= quantity) // if ingredient qt sufficient
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

        public static bool CookRecipe(IRecipe recipe)
        {
            List<(Item item, int quantity)> ingredientList = recipe.GetIngredientList();
        
            if (InventoryContainsAllIngredients(ingredientList))
            {
                // remove used items
                foreach (var (item, quantity) in ingredientList)
                {
                    GameStateManager.Instance.RemoveItemFromInventory(item, quantity);
                }
            
                // add recipe result to inventory
                GameStateManager.Instance.AddItemToInventory(recipe.GetRecipeResult(), 1);
                return true;
            }
            return false;
        }
        
        #region CookingRecipes
        public static void CookBurger()
        {
            CookRecipe(new Recipes.Hamburger());
        }

        public static void CookSalad()
        {
            CookRecipe(new Recipes.Salad());
        }

        public static void CookSushi()
        {
            CookRecipe(new Recipes.Sushi());
        }
        #endregion CookingRecipes
    }
}
