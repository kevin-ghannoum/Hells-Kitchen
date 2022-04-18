using System.Linq;
using System.Reflection;
using Enums.Items;
using PlayerInventory;

namespace Common.Enums.Items
{
    public static class Items
    {
        // Ingredients
        public static readonly Item Honey = new Item(ItemInstance.Honey, "Honey", 1.0f, ItemModel.Honey);
        public static readonly Item Meat = new Item(ItemInstance.Meat, "Meat", 1.0f, ItemModel.Meat);
        public static readonly Item Fish = new Item(ItemInstance.Fish, "Fish", 1.0f, ItemModel.Fish);
        public static readonly Item Mushroom = new Item(ItemInstance.Mushroom, "Mushroom", 1.0f, ItemModel.Mushroom);

        // Recipe results
        public static readonly Item Hamburger = new Item(ItemInstance.Hamburger, "Hamburger", 1.0f, ItemModel.Burger);
        public static readonly Item Salad = new Item(ItemInstance.Salad, "Salad", 1.0f, ItemModel.Salad);
        public static readonly Item Sushi = new Item(ItemInstance.Sushi, "Sushi", 1.0f, ItemModel.Sushi);

        public static Item GetItem(ItemInstance item)
        {
            FieldInfo field = typeof(Items).GetFields().FirstOrDefault(f => f.Name == item.ToString());
            return (Item)field?.GetValue(null);
        }
    }
}
