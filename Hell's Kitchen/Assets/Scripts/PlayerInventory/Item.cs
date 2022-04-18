using Enums.Items;

namespace PlayerInventory
{
    public class Item
    {
        public readonly ItemInstance ItemInstance;
        public readonly string Name;
        public readonly float Rarity;
        public readonly ItemModel ItemModel;

        public Item(ItemInstance itemInstance, string name, float rarity, ItemModel itemModel)
        {
            ItemInstance = itemInstance;
            Name = name;
            Rarity = rarity;
            ItemModel = itemModel;
        }
    }
}
