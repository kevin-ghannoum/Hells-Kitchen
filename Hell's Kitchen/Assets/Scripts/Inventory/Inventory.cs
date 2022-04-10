using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // hold a dictionary of items and quantities
    Dictionary<Item, int> itemDictionary = new Dictionary<Item, int>();

    public Dictionary<Item, int> GetInventory()
    {
        return itemDictionary;
    }

    // increment value if already in inventory else add
    public void AddItemToInventory(Item item, int quantity)
    {
        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item] += quantity;
        }
        else
        {
            itemDictionary.Add(item, quantity);
        }
    }

    // decrement value if (value-1) > 0 else remove
    public void RemoveItemFromInventory(Item item, int quantity)
    {
        if (itemDictionary.ContainsKey(item))
        {
            if (itemDictionary[item] - 1 <= 0)
            {
                itemDictionary.Remove(item);
            }
            else
            {
                itemDictionary[item] -= quantity;   
            }
        }
    }
}
