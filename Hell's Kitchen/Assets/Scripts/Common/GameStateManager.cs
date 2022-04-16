using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour
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
        public bool dungeonTimeHasElapsed = false;

        public Dictionary<IRecipe, int> OrderList =  new Dictionary<IRecipe, int>();

        public List<string> PurchasedWeapons =  new List<string>();
        
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
            PurchasedWeapons = new List<string>();
        }
    }
}
