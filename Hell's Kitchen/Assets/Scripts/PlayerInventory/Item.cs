using Enums.Items;

namespace PlayerInventory
{
    public class Item
    {
        public readonly ItemType ItemType;
        public readonly string Name;
        public readonly float Rarity;
        public readonly ItemModel ItemModel;

        public Item(ItemType itemType, string name, float rarity, ItemModel itemModel)
        {
            ItemType = itemType;
            Name = name;
            Rarity = rarity;
            ItemModel = itemModel;
        }
    }
}