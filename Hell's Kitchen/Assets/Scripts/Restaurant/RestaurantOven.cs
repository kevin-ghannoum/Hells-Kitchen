using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Enums.Items;
using Dungeon_Generation;
using Enums.Items;
using Input;
using Photon.Pun;
using PlayerInventory.Cooking;
using UI;
using UnityEngine;

namespace Restaurant
{
    public class RestaurantOven : Interactable
    {
        [SerializeField] private AudioClip cookingSound;
        [SerializeField] private PhotonView photonView;
        
        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            // TODO Remove after feature complete
            DebugAddInventoryAndOrders();
        }

        protected override void Interact()
        {
            var orderList = RestaurantManager.Instance.OrderList.Where(o => !o.Served);
            var player = GameObject.FindWithTag(Tags.Player);

            Dictionary<ItemInstance, int> neededItems = new Dictionary<ItemInstance, int>();
            foreach (var order in orderList)
            {
                if (neededItems.ContainsKey(order.Item))
                    neededItems[order.Item] += order.Quantity;
                else
                    neededItems.Add(order.Item, order.Quantity);
            }

            foreach (var item in GameStateData.inventory.GetInventoryItems())
            {
                if (neededItems.ContainsKey(item.Key))
                    neededItems[item.Key] -= item.Value;
                else
                    neededItems.Add(item.Key, -item.Value);
            }

            foreach (var item in neededItems)
            {
                if (item.Value > 0 && Cooking.CookRecipe(Cooking.GetItemRecipe(item.Key), item.Value))
                {
                    AdrenalinePointsUI.SpawnIngredientString(player.transform.position, $"+{item.Value} {Items.GetItem(item.Key).Name}");
                }
            }

            if (photonView.IsMine && neededItems.Any(item => item.Value > 0))
            {
                photonView.RPC(nameof(PlayCookingSoundRPC), RpcTarget.All);
            }
        }

        [PunRPC]
        private void PlayCookingSoundRPC()
        {
            AudioSource.PlayClipAtPoint(cookingSound, transform.position);
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
