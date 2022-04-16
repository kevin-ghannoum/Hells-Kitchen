using System.Collections.Generic;
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
        public GameObject carriedWeapon;
        public bool IsCarryingWeapon => carriedWeapon != null;
        public float cashMoney;
        public bool dungeonTimeHasElapsed;

        public Dictionary<IRecipe, int> OrderList;

        public List<string> PurchasedWeapons;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }

            DontDestroyOnLoad(Instance.gameObject);
        }

        private void Initialize()
        {
            playerMaxHitPoints = 100f;
            playerCurrentHitPoints = playerMaxHitPoints;
            playerMaxStamina = 5f;
            playerCurrentStamina = playerMaxStamina;
            cashMoney = 0f;
            carriedWeapon = null;
            OrderList = new Dictionary<IRecipe, int>();
            PurchasedWeapons = new List<string>();
        }

        public bool IsLowHP()
        {
            return ((playerCurrentHitPoints / playerMaxHitPoints) * 100) < 60;
        }
    }
}
