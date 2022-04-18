using System;
using System.Collections.Generic;
using Enums.Items;
using Player;
using UnityEngine;

namespace PlayerInventory.Cooking
{
    public static class Cooking
    {
        private static bool InventoryContainsAllIngredients(List<(Item item, int quantity)> ingredientList)
        {
            if (PlayerController.Instance.GetPlayerInventory().GetInventoryItems().Count == 0)
                return false;
        
            Dictionary<Item, bool> ingredientCheckList = new Dictionary<Item, bool>();
            foreach (var (item, quantity) in ingredientList)
            {
                if (PlayerController.Instance.GetPlayerInventory().GetInventoryItems().ContainsKey(item)) // if ingredient in inventory
                {
                    if (PlayerController.Instance.GetPlayerInventory().GetInventoryItems()[item] >= quantity) // if ingredient qt sufficient
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
                    PlayerController.Instance.RemoveItemFromInventory(item, quantity);
                }
            
                // add recipe result to inventory
                PlayerController.Instance.AddItemToInventory(recipe.GetRecipeResult(), 1);
                return true;
            }
            return false;
        }
        
        public static IRecipe GetItemRecipe(ItemInstance item)
        {
            return (IRecipe)Activator.CreateInstance(Type.GetType($"PlayerInventory.Cooking.Recipes+{item}"));
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
