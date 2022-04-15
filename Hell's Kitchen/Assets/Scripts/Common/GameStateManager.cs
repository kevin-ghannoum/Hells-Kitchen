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

        public Dictionary<IRecipe, int> OrderList;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(Instance.gameObject);
            Initialize();
        }

        private void Initialize()
        {
            playerMaxHitPoints = 100f;
            playerCurrentHitPoints = playerMaxHitPoints;
            cashMoney = 0f;
            carriedWeapon = null;
            OrderList = new Dictionary<IRecipe, int>();
        }
    }
}
