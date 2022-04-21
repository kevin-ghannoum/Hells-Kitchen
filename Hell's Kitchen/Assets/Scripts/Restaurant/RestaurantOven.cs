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
        [SerializeField] private AudioClip insufficientSound;
        [SerializeField] private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
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

            var neededItemsCopy = neededItems.ToDictionary(e => e.Key, e => e.Value);

            foreach (var item in neededItems)
            {
                var amountCooked = 0;
                for (var i = 0; i < item.Value; i++)
                {
                    if (Cooking.CookRecipe(Cooking.GetItemRecipe(item.Key)))
                    {
                        amountCooked++;
                        neededItemsCopy[item.Key]--;
                    }
                }

                if (amountCooked > 0)
                {
                    AdrenalinePointsUI.SpawnIngredientString(player.transform.position, $"+{amountCooked} {Items.GetItem(item.Key).Name}");
                }
            }

            if (neededItemsCopy.Any(item => item.Value > 0))
            {
                photonView.RPC(nameof(PlayInsufficientSoundRPC), RpcTarget.All);
            }
            else if (neededItems.Any(item => item.Value > 0))
            {
                photonView.RPC(nameof(PlayCookingSoundRPC), RpcTarget.All);
            }
        }

        [PunRPC]
        private void PlayInsufficientSoundRPC()
        {
            AudioSource.PlayClipAtPoint(insufficientSound, transform.position);
        }

        [PunRPC]
        private void PlayCookingSoundRPC()
        {
            AudioSource.PlayClipAtPoint(cookingSound, transform.position);
        }
    }
}
