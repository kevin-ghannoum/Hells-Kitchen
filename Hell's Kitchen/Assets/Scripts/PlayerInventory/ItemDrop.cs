using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Player;
using UnityEngine;

namespace PlayerInventory
{
    public class ItemDrop : MonoBehaviour
    {
        private Item item;

        private Dictionary<string, Item> allItems;
        private void Start()
        {
            allItems = 
                typeof(Items).GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(f => f.Name, f => (Item)f.GetValue(this));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();

                if (allItems.TryGetValue(gameObject.name, out item))
                {
                    player.AddItemToInventory(item,  1);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("Added item must be part of the defined list of Items");
                }
            }
        }
    }
}
