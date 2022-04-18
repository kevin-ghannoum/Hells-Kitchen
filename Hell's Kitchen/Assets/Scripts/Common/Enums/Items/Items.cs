using System.Linq;
using System.Reflection;
using PlayerInventory;
using UnityEditor;
using UnityEngine;

namespace Enums.Items
{
    public static class Items
    {
        // Ingredients
        public static readonly Item Honey = new Item(ItemType.Ingredient, "Honey", 1.0f, ItemModel.Honey);
        public static readonly Item Meat = new Item(ItemType.Ingredient, "Meat", 1.0f, ItemModel.Meat);
        public static readonly Item Fish = new Item(ItemType.Ingredient, "Fish", 1.0f, ItemModel.Fish);
        public static readonly Item Mushroom = new Item(ItemType.Ingredient, "Mushroom", 1.0f, ItemModel.Mushroom);

        // Recipe results
        public static readonly Item Hamburger = new Item(ItemType.RecipeResult, "Hamburger", 1.0f, ItemModel.Burger);
        public static readonly Item Salad = new Item(ItemType.RecipeResult, "Salad", 1.0f, ItemModel.Salad);
        public static readonly Item Sushi = new Item(ItemType.RecipeResult, "Sushi", 1.0f, ItemModel.Sushi);

        public static Item GetItem(ItemInstance item)
        {
            FieldInfo field = typeof(Items).GetFields().FirstOrDefault(f => f.Name == item.ToString());
            return (Item)field?.GetValue(null);
        }
    }
}
