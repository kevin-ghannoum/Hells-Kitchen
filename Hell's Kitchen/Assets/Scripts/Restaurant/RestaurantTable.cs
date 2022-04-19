using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Enums.Items;
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
                    ServeOrder(order);
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

        private void ServeOrder(RestaurantOrder order)
        {
            if (GameStateData.inventory.HasItem(order.Item, order.Quantity))
            {
                // Remove items and give money
                GameStateManager.RemoveItemFromInventory(order.Item, order.Quantity);
                GameStateManager.SetCashMoney(GameStateData.cashMoney + order.CashMoney);
                
                // Spawn some adrenaline points
                var position = transform.position + 2.0f * Vector3.up;
                AdrenalinePointsUI.SpawnIngredientString(position, $"-{order.Quantity} {Items.GetItem(order.Item).Name}");
                AdrenalinePointsUI.SpawnGoldNumbers(position, order.CashMoney);
                
                // Set served
                order.Served = true;
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
                Quantity = Random.Range(1, 4),
                CashMoney = Random.Range(20, 40)
            };
        }
    }
}
