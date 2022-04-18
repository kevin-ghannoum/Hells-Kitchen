using System.Linq;
using Common;
using Common.Enums;
using Enums.Items;
using Input;
using Photon.Pun;
using Player;
using UnityEngine;

namespace PlayerInventory.Cooking
{
    public class Oven : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

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

        [PunRPC]
        private void AutoCraftOrderedRecipes()
        {
            // TODO: take whatever is on Cristian's branch
            // var orderList = GameStateManager.Instance.OrderList;
            // var keys = orderList.Keys.ToList();
            var totalIncome = 0f;
            // foreach (var key in keys)
            // {
            //     int numToCraft = orderList[key];
            //     for (int i = 0; i < numToCraft; i++)
            //     {
            //         if (Cooking.CookRecipe(key))
            //         {
            //             // Get money based on order
            //             totalIncome += key.GetCost();
            //             
            //             // Decrement number of recipes available to cook
            //             if (orderList[key] > 1)
            //                 orderList[key]--;
            //             else
            //                 orderList.Remove(key);
            //         }
            //     }
            // }
            
            GameStateManager.SetCashMoney(GameStateData.cashMoney + totalIncome);
        }
    }
}
