using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Enums.Items;
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
            if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
            {
                AutoCraftOrderedRecipes();
            }
        }

        private void AutoCraftOrderedRecipes()
        {
            var orderList = RestaurantManager.Instance.OrderList.Where(o => !o.Served);
            var player = GameObject.FindWithTag(Tags.Player);
            foreach (var order in orderList)
            {
                for (int i = 0; i < order.Quantity; i++)
                {
                    if (Cooking.CookRecipe(Cooking.GetItemRecipe(order.Item)))
                    {
                        AdrenalinePointsUI.SpawnIngredientString(player.transform.position, $"+{order.Quantity} {Items.GetItem(order.Item).Name}");
                    }
                }
            }
        }

        private void DebugAddInventoryAndOrders()
        {
            // TODO Remove After Testing
            GameStateManager.AddItemToInventory(ItemInstance.Fish, 20);
            GameStateManager.AddItemToInventory(ItemInstance.Honey, 20);
            GameStateManager.AddItemToInventory(ItemInstance.Mushroom, 20);
            GameStateManager.AddItemToInventory(ItemInstance.Meat, 20);
        }
    }
}
