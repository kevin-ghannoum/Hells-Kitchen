using UnityEngine;

namespace Common
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;
    
        // Player
        public float playerMaxHitPoints;
        public float playerCurrentHitPoints;
        public float cashMoney;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(Instance.gameObject);
            Initalize();
        }

        private void Initalize()
        {
            playerMaxHitPoints = 100f;
            playerCurrentHitPoints = playerMaxHitPoints;
            cashMoney = 0f;
        }
    }
}
