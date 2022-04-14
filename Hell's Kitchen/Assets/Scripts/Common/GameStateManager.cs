using System.Collections.Generic;
using Common.Interfaces;
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
        
        public bool IsCarryingWeapon => carriedWeapon != null;
        public float cashMoney;

        public Dictionary<IRecipe, int> OrderList;

        public List<GameObject> PurchasedWeapons;
        
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
            cashMoney = 10000f;
            carriedWeapon = null;
            OrderList = new Dictionary<IRecipe, int>();
            PurchasedWeapons = new List<GameObject>();
        }
    }
}
