using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // hold a dictionary of items and quantities
    Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    public Dictionary<Item, int> GetInventory()
    {
        return inventory;
    }

    // increment value if already in inventory else add
    public void AddItemToInventory(Item item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory.Add(item, quantity);
        }
    }

    // decrement value if (value-1) > 0 else remove
    public void RemoveItemFromInventory(Item item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            if (inventory[item] - 1 < 0)
            {
                inventory.Remove(item);
            }

            inventory[item] -= quantity;
        }
    }
}
