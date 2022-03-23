using Enums;

public class Item
{
    public readonly ItemType itemType;
    public readonly string name;
    public readonly float rarity;
    public readonly ItemModel ItemModel;

    public Item(ItemType itemType, string name, float rarity, ItemModel itemModel)
    {
        this.itemType = itemType;
        this.name = name;
        this.rarity = rarity;
        this.ItemModel = itemModel;
    }
}