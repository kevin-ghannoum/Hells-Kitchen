using System.Linq;
using Common;
using Common.Enums;
using Input;
using Player;
using UnityEngine;

namespace PlayerInventory.Cooking
{
    public class AutoCookMeal : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

        private void Start()
        {
            DebugAddInventoryAndOrders();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && _input.dropItem)
            {
                if (_input.interact)
                {
                    AutoCraftOrderedRecipes();
                }
            }
        }

        private void AutoCraftOrderedRecipes()
        {
            var orderList = GameStateManager.Instance.OrderList;
            var keys = orderList.Keys.ToList();
            var totalIncome = 0f;
            foreach (var key in keys)
            {
                int numToCraft = orderList[key];
                for (int i = 0; i < numToCraft; i++)
                {
                    if (Cooking.CookRecipe(key))
                    {
                        // Get money based on order
                        totalIncome += key.GetCost();
                        
                        // Decrement number of recipes available to cook
                        if (orderList[key] > 1)
                            orderList[key]--;
                        else
                            orderList.Remove(key);
                    }
                }
            }
            
            GameStateManager.Instance.cashMoney += totalIncome;
        }

        private void DebugAddInventoryAndOrders()
        {
            // TODO Remove After Testing
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Fish, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Honey, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.Mushroom, 20);
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().AddItemToInventory(Items.PorkChop, 20);
            
            GameStateManager.Instance.OrderList.Add(new Recipes.Hamburger(), 1);
            GameStateManager.Instance.OrderList.Add(new Recipes.Salad(), 2);
            GameStateManager.Instance.OrderList.Add(new Recipes.Sushi(), 5);
        }
    }
}