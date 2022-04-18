using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Enums.Items;
using Input;
using Player;
using PlayerInventory.Cooking;
using UI;
using UnityEngine;

namespace Restaurant
{
    public class RestaurantOven : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

        private void Start()
        {
            // TODO Remove after feature complete
            DebugAddInventoryAndOrders();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && _input.WasPressedThisFrame(InputManager.Actions.Interact))
            {
                AutoCraftOrderedRecipes();
            }
        }

        private void AutoCraftOrderedRecipes()
        {
            var orderList = RestaurantManager.Instance.OrderList.Where(o => !o.Served);
            foreach (var order in orderList)
            {
                for (int i = 0; i < order.Quantity; i++)
                {
                    Cooking.CookRecipe(Cooking.GetItemRecipe(order.Item));
                }
            }
        }

        private void DebugAddInventoryAndOrders()
        {
            // TODO Remove After Testing
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Fish, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Honey, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Mushroom, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Meat, 20);
        }
    }
}
