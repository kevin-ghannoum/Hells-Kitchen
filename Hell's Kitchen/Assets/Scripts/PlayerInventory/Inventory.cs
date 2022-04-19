using System;
using System.Collections.Generic;
using System.Linq;
using Enums.Items;

namespace PlayerInventory
{
    [Serializable]
    public class Inventory
    {
        // hold a dictionary of items and quantities
        private Dictionary<ItemInstance, int> _itemDictionary = new Dictionary<ItemInstance, int>();

        public void SetInventoryItems(Dictionary<ItemInstance, int> items)
        {
            _itemDictionary = items;
        }

        public Dictionary<ItemInstance, int> GetInventoryItems()
        {
            return _itemDictionary;
        }

        // increment value if already in inventory else add
        public void AddItemToInventory(ItemInstance item, int quantity)
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
        public void RemoveItemFromInventory(ItemInstance item, int quantity)
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

        public bool HasItem(ItemInstance item, int quantity)
        {
            return _itemDictionary.ContainsKey(item) && _itemDictionary[item] >= quantity;
        }
    }
}
