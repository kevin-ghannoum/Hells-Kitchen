using System.Collections.Generic;
using PlayerInventory;

public class Inventory
{
    // hold a dictionary of items and quantities
    private readonly Dictionary<Item, int> _itemDictionary = new Dictionary<Item, int>();

    public Dictionary<Item, int> GetInventoryItems()
    {
        return _itemDictionary;
    }

    // increment value if already in inventory else add
    public void AddItemToInventory(Item item, int quantity)
    {
        if (_itemDictionary.ContainsKey(item))
        {
            _itemDictionary[item] += quantity;
        }
        else
        {
            _itemDictionary.Add(item, quantity);
        }
    }

    // decrement value if (value-1) > 0 else remove
    public void RemoveItemFromInventory(Item item, int quantity)
    {
        if (_itemDictionary.ContainsKey(item))
        {
            if (_itemDictionary[item] - 1 <= 0)
            {
                _itemDictionary.Remove(item);
            }
            else
            {
                _itemDictionary[item] -= quantity;   
            }
        }
    }
}