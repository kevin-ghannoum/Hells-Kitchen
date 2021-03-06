using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Enums.Items;
using Dungeon_Generation;
using Enums.Items;
using Input;
using Photon.Pun;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantTable : Interactable, IPunObservable
    {
        [SerializeField]
        private RestaurantSeat[] seats;

        [SerializeField]
        private ProximityToggleUI interactUI;

        [SerializeField]
        private ProximityToggleUI restaurantUI;

        [SerializeField]
        private GameObject orderItemPrefab;

        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private AudioClip rawSound;

        public List<RestaurantOrder> OrderList = new List<RestaurantOrder>();
        private readonly Dictionary<int, RestaurantOrderItem> _orderUIObjects = new Dictionary<int, RestaurantOrderItem>();

        private void Reset()
        {
            seats = GetComponentsInChildren<RestaurantSeat>();
            photonView = GetComponent<PhotonView>();
        }

        public override void Update()
        {
            base.Update();
            restaurantUI.IsDisabled = !seats.Any(s => s.IsSitting);
            interactUI.IsDisabled = OrderList.All(o => o.Served);
        }

        public void OnCustomerSit()
        {
            if (photonView.IsMine)
            {
                OrderList.Add(GenerateRandomOrder());
                RefreshOrderUI();
            }
        }

        private void RefreshOrderUI()
        {
            foreach (var order in OrderList)
            {
                RestaurantOrderItem orderItem;
                if (_orderUIObjects.ContainsKey(order.ID))
                {
                    orderItem = _orderUIObjects[order.ID];
                }
                else
                {
                    orderItem =
                        Instantiate(orderItemPrefab, restaurantUI.gameObject.transform.Find("RestaurantOrderUI/Canvas/Container/InnerContainer"))
                        .GetComponent<RestaurantOrderItem>();
                    _orderUIObjects.Add(order.ID, orderItem);
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
            else if (!order.Served)
            {
                photonView.RPC(nameof(PlayRawSoundRPC), RpcTarget.All);
            }
        }

        [PunRPC]
        private void PlayRawSoundRPC()
        {
            AudioSource.PlayClipAtPoint(rawSound, transform.position);
        }

        [PunRPC]
        private void ServeOrdersRPC()
        {
            foreach (var restaurantOrder in OrderList)
            {
                ServeOrder(restaurantOrder);
            }
            RefreshOrderUI();
        }

        protected override void Interact()
        {
            photonView.RPC(nameof(ServeOrdersRPC), RpcTarget.All);
        }

        private RestaurantOrder GenerateRandomOrder()
        {
            ItemInstance[] possibleValues = {
                ItemInstance.Hamburger,
                ItemInstance.Salad,
                ItemInstance.Sushi
            };

            var quantity = Random.Range(1, 3);

            return new RestaurantOrder()
            {
                Item = possibleValues[Random.Range(0, possibleValues.Length)],
                Quantity = quantity,
                CashMoney = (Random.Range(RestaurantManager.Instance.MinOrderPrice, RestaurantManager.Instance.MaxOrderPrice) * quantity)
            };
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(OrderList.ToArray());
            }
            else if (stream.IsReading)
            {
                OrderList = ((RestaurantOrder[])stream.ReceiveNext()).ToList();
                RefreshOrderUI();
            }
        }
    }
}
