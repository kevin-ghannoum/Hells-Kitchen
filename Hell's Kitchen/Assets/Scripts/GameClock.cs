using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameClock : MonoBehaviour
    {
        public static GameClock Instance;

        [SerializeField] private float DungeonTime;
        private float _elapsedTime = 0f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(Instance.gameObject);
        }

        private void Update()
        {
            if (_elapsedTime < DungeonTime)
                _elapsedTime += Time.deltaTime;
        }
    }
}