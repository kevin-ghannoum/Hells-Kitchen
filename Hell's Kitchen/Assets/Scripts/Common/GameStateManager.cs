using System.Collections.Generic;
using Enums.Items;
using Photon.Pun;
using PlayerInventory;
using UI;
using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour, IPunObservable
    {
        [SerializeField] public PhotonView photonView;
        public static GameStateManager Instance;

        public InventoryUI inventoryUI;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            photonView = GetComponent<PhotonView>();
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
                stream.SendNext(GameStateData.inventory);
                stream.SendNext(GameStateData.cashMoney);
            }
            else if(stream.IsReading)
            {
                GameStateData.inventory = (Inventory) stream.ReceiveNext();
                GameStateData.cashMoney = (float) stream.ReceiveNext();
            }
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
            GameStateData.inventory.AddItemToInventory(Items.GetItem(itemInstance), quantity);
            inventoryUI.UpdateInventory(GameStateData.inventory.GetInventoryItems());
        }

        [PunRPC]
        public void RemoveItemFromInventoryRPC(ItemInstance itemInstance, int quantity)
        {
            GameStateData.inventory.RemoveItemFromInventory(Items.GetItem(itemInstance), quantity);
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
            Instance.photonView.RPC(nameof(AddItemToInventoryRPC), RpcTarget.MasterClient, itemInstance, quantity);
        }
        
        public static void RemoveItemFromInventory(ItemInstance itemInstance, int quantity)
        {
            Instance.photonView.RPC(nameof(RemoveItemFromInventoryRPC), RpcTarget.MasterClient, itemInstance, quantity);
        }

        public static void SetCashMoney(float value)
        {
            Instance.photonView.RPC(nameof(SetCashMoneyRPC), RpcTarget.MasterClient, value);
        }

        public static void AddPurchasedWeapon(string weaponName)
        {
            Instance.photonView.RPC(nameof(AddPurchasedWeaponRPC), RpcTarget.MasterClient, weaponName);
        }
    }
}
