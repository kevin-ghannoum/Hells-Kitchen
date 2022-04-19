using System.Collections.Generic;
using System.Linq;
using Enums.Items;
using ExitGames.Client.Photon;
using Photon.Pun;
using PlayerInventory;
using Restaurant;
using Server;
using UI;
using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour, IPunObservable
    {
        public static GameStateManager Instance;
        [SerializeField] public PhotonView photonView;
        [SerializeField] public InventoryUI inventoryUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            photonView = GetComponent<PhotonView>();
            inventoryUI = FindObjectOfType<InventoryUI>();
            PhotonPeer.RegisterType(typeof(RestaurantOrder), (byte)'O', RestaurantOrder.Serialize, RestaurantOrder.Deserialize);
        }

        private void ResetDefaults()
        {
            GameStateData.playerMaxHitPoints = 100f;
            GameStateData.playerCurrentHitPoints = GameStateData.playerMaxHitPoints;
            GameStateData.playerMaxStamina = 5f;
            GameStateData.playerCurrentStamina = GameStateData.playerMaxStamina;
            GameStateData.cashMoney = 0f;
            GameStateData.purchasedWeapons = new List<string>();
            GameStateData.dungeonTimeHasElapsed = true;
        }

        #region PUNCallbacks

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                var inventoryItems = GameStateData.inventory.GetInventoryItems();
                var serializedInventoryItems = 
                    inventoryItems.Keys
                        .ToDictionary(k => (int) k, k => inventoryItems[k]);
                stream.SendNext(serializedInventoryItems);
                stream.SendNext(GameStateData.cashMoney);
            }
            else if (stream.IsReading)
            {
                var serializedInventoryItems = (Dictionary<int, int>) stream.ReceiveNext();
                var inventoryItems = 
                    serializedInventoryItems.Keys
                        .ToDictionary(k => (ItemInstance) k, k => serializedInventoryItems[k]);
                GameStateData.inventory.SetInventoryItems(inventoryItems);
                GameStateData.cashMoney = (float)stream.ReceiveNext();
                inventoryUI.UpdateInventory(inventoryItems);
            }
        }
        
        [PunRPC]
        public void SetGameObjectActiveByNameRPC(string objectName)
        {
            var weaponCollection = GameObject.Find("WeaponCollection");
            var obj = weaponCollection.transform.Find(objectName);
            if (!obj)
                return;
            
            obj.gameObject.SetActive(true);
        }

        #endregion

        #region SharedState

        public Inventory GetPlayerInventory()
        {
            return GameStateData.inventory;
        }

        [PunRPC]
        public void AddItemToInventoryRPC(ItemInstance itemInstance, int quantity)
        {
            GameStateData.inventory.AddItemToInventory(itemInstance, quantity);
            inventoryUI.UpdateInventory(GameStateData.inventory.GetInventoryItems());
        }

        [PunRPC]
        public void RemoveItemFromInventoryRPC(ItemInstance itemInstance, int quantity)
        {
            GameStateData.inventory.RemoveItemFromInventory(itemInstance, quantity);
            inventoryUI.UpdateInventory(GameStateData.inventory.GetInventoryItems());
        }

        [PunRPC]
        public void SetCashMoneyRPC(float value)
        {
            GameStateData.cashMoney = value;
        }

        [PunRPC]
        public void AddPurchasedWeaponRPC(string weaponName)
        {
            GameStateData.purchasedWeapons.Add(weaponName);
        }

        #endregion

        public static void AddItemToInventory(ItemInstance itemInstance, int quantity)
        {
            Instance.photonView.RPC(nameof(AddItemToInventoryRPC), RpcTarget.All, itemInstance, quantity);
        }

        public static void RemoveItemFromInventory(ItemInstance itemInstance, int quantity)
        {
            Instance.photonView.RPC(nameof(RemoveItemFromInventoryRPC), RpcTarget.All, itemInstance, quantity);
        }

        public static void SetCashMoney(float value)
        {
            Instance.photonView.RPC(nameof(SetCashMoneyRPC), RpcTarget.All, value);
        }

        public static void AddPurchasedWeapon(string weaponName)
        {
            Instance.photonView.RPC(nameof(AddPurchasedWeaponRPC), RpcTarget.All, weaponName);
        }
    }
}
