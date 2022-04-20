using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Enums.Items;
using Enums.Items;
using Input;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantTable : MonoBehaviour, IPunObservable
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

        private InputManager _input => InputManager.Instance;
        public List<RestaurantOrder> OrderList = new List<RestaurantOrder>();
        private readonly Dictionary<int, RestaurantOrderItem> _orderUIObjects = new Dictionary<int, RestaurantOrderItem>();

        private void Reset()
        {
            seats = GetComponentsInChildren<RestaurantSeat>();
            photonView = GetComponent<PhotonView>();
        }

        private void Update()
        {
            restaurantUI.IsDisabled = !seats.Any(s => s.IsSitting);
            interactUI.IsDisabled = OrderList.All(o => o.Served);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _input.reference.actions["Interact"].performed += ServeOrders;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _input.reference.actions["Interact"].performed -= ServeOrders;
                }
            }
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
            else
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

        private void ServeOrders(InputAction.CallbackContext context)
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

            return new RestaurantOrder()
            {
                Item = possibleValues[Random.Range(0, possibleValues.Length)],
                Quantity = Random.Range(1, 4),
                CashMoney = Random.Range(20, 40)
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
