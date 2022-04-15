using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;

        // Player
        public float playerMaxHitPoints;
        public float playerCurrentHitPoints;
        public GameObject carriedWeapon;

        public float maxSprintTime = 5f;
        public float elapsedSprintTime = 0f;
        
        public bool dungeonTimeElapsed = true;
        
        public bool IsCarryingWeapon => carriedWeapon != null;
        public float cashMoney;

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
            cashMoney = 0f;
            carriedWeapon = null;
            OrderList = new Dictionary<IRecipe, int>();
            PurchasedWeapons = new List<string>();
        }
    }
}
