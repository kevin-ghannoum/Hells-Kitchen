using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Enums.Items;
using Input;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantTable : MonoBehaviour
    {
        [SerializeField]
        private RestaurantSeat[] seats;

        [SerializeField]
        private ProximityToggleUI interactUI;

        [SerializeField]
        private ProximityToggleUI restaurantUI;

        [SerializeField]
        private GameObject orderItemPrefab;

        private InputManager _input => InputManager.Instance;
        public readonly List<RestaurantOrder> OrderList = new List<RestaurantOrder>();
        
        private void Reset()
        {
            seats = GetComponentsInChildren<RestaurantSeat>();
        }

        private void Update()
        {
            restaurantUI.IsDisabled = !seats.Any(s => s.IsSitting);
            interactUI.IsDisabled = OrderList.All(o => o.Served);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player) && _input.interact)
            {
                foreach (var order in OrderList)
                {
                    if (GameStateData.inventory.HasItem(order.Item, order.Quantity))
                    {
                        GameStateManager.RemoveItemFromInventory(order.Item, order.Quantity);
                        order.Served = true;
                    }
                }
                RefreshOrderUI();
            }
        }

        public void OnCustomerSit()
        {
            OrderList.Add(GenerateRandomOrder());
            RefreshOrderUI();
        }

        private void RefreshOrderUI()
        {
            foreach (var order in OrderList)
            {
                RestaurantOrderItem orderItem;
                if (order.UIObject != null)
                {
                    orderItem = order.UIObject;
                }
                else
                {
                    orderItem = order.UIObject =
                        Instantiate(orderItemPrefab, restaurantUI.gameObject.transform.Find("RestaurantOrderUI/Canvas/Container/InnerContainer"))
                        .GetComponent<RestaurantOrderItem>();
                }
                orderItem.Item = order.Item;
                orderItem.Quantity = order.Quantity;
                orderItem.Served = order.Served;
                orderItem.RefreshUI();
            }
        }

        private RestaurantOrder GenerateRandomOrder()
        {
            ItemInstance[] possibleValues = {
                ItemInstance.Hamburger,
                ItemInstance.Salad,
                ItemInstance.Sushi
            };
            
            return new RestaurantOrder() {
                Item = possibleValues[Random.Range(0, possibleValues.Length)],
                Quantity = Random.Range(1, 4)
            };
        }
    }
}
