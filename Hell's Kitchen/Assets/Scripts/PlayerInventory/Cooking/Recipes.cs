using System.Collections.Generic;
using Common.Enums.Items;
using Enums.Items;

namespace PlayerInventory.Cooking
{
    public static class Recipes
    {
        public class Hamburger : IRecipe
        {
            public List<(Item item, int quantity)> GetIngredientList()
            {
                return new List<(Item item, int quantity)>(){
                    (Items.Meat, 1),
                    (Items.Mushroom, 1)
                };
            }

            public Item GetRecipeResult()
            {
                return Items.Hamburger;
            }

            public float GetCost()
            {
                return 10f;
            }
        }

        public class Salad : IRecipe
        {
            public List<(Item item, int quantity)> GetIngredientList()
            {
                return new List<(Item item, int quantity)>(){
                    (Items.Mushroom, 2),
                    (Items.Honey, 1)
                };
            }

            public Item GetRecipeResult()
            {
                return Items.Salad;
            }

            public float GetCost()
            {
                return 10f;
            }
        }

        public class Sushi : IRecipe
        {
            public List<(Item item, int quantity)> GetIngredientList()
            {
                return new List<(Item item, int quantity)>(){
                    (Items.Fish, 2)
                };
            }

            public Item GetRecipeResult()
            {
                return Items.Sushi;
            }

            public float GetCost()
            {
                return 10f;
            }
        }
    }
}