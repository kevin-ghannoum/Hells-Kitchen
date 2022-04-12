using Common;
using Common.Enums;
using Input;
using UnityEngine;

namespace PlayerInventory.Cooking
{
    public class AutoCookMeal : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && _input.dropItem)
            {
                var orderList = GameStateManager.Instance.OrderList;
                foreach (var order in orderList)
                {
                    int numToCraft = order.Value;
                    for (int i = 0; i < numToCraft; i++)
                    {
                        Cooking.CookRecipe(order.Key);
                    }
                }
                
            }
        }
    }
}