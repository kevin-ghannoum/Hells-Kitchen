using System.Collections.Generic;
using Common.Enums;
using Photon.Pun;
using PlayerInventory;
using UI;
using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour, IPunObservable
    {
        public static GameStateManager Instance;

        // Player
        public float playerMaxHitPoints = 100f;
        public float playerCurrentHitPoints = 100f;
        public float playerMaxStamina = 5f;
        public float playerCurrentStamina = 5f;
        public WeaponInstance carriedWeapon = WeaponInstance.None;
        public bool IsCarryingWeapon => (carriedWeapon != WeaponInstance.None);
        public float cashMoney = 0f;
        public bool dungeonTimeHasElapsed = true;

        public Dictionary<IRecipe, int> OrderList =  new Dictionary<IRecipe, int>();
        public List<string> purchasedWeapons =  new List<string>();
        
        public Inventory inventory = new Inventory();
        public InventoryUI inventoryUI;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            DontDestroyOnLoad(Instance.gameObject);
        }

        private void ResetDefaults()
        {
            playerMaxHitPoints = 100f;
            playerCurrentHitPoints = playerMaxHitPoints;
            playerMaxStamina = 5f;
            playerCurrentStamina = playerMaxStamina;
            cashMoney = 0f;
            OrderList = new Dictionary<IRecipe, int>();
            purchasedWeapons = new List<string>();
            dungeonTimeHasElapsed = true;
        }

        #region PUNCallbacks

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(inventory);
                stream.SendNext(cashMoney);
            }
            else if(stream.IsReading)
            {
                stream.ReceiveNext();
            }
        }

        #endregion

        #region PlayerInventory

        public Inventory GetPlayerInventory()
        {
            return inventory;
        }

        public void AddItemToInventory(Item item, int quantity)
        {
            inventory.AddItemToInventory(item, quantity);
            inventoryUI.UpdateInventory(inventory.GetInventoryItems());
        }

        public void RemoveItemFromInventory(Item item, int quantity)
        {
            inventory.RemoveItemFromInventory(item, quantity);
            inventoryUI.UpdateInventory(inventory.GetInventoryItems());
        }

        #endregion
    }
}
