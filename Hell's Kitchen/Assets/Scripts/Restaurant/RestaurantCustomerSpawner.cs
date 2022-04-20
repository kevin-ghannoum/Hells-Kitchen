using System;
using Common;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantCustomerSpawner : MonoBehaviour
    {
        public static RestaurantCustomerSpawner Instance;

        [SerializeField]
        private GameObject[] customerPrefabs;

        [SerializeField]
        private Transform[] spawnPoints;

        [SerializeField]
        private float spawnDelay = 1.5f;

        [SerializeField] private int maxCustomers = 25;

        private void Awake()
        {
            // Singleton instance
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var randomSpawn = CalculateNumCustomers();
                for (int i = 0; i < randomSpawn; i++)
                {
                    Invoke(nameof(SpawnCustomer), spawnDelay * i + Random.Range(-0.5f, 0.5f));
                }
            }
        }

        private void SpawnCustomer()
        {
            // Random spawn point
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Random customer prefab
            var customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];

            // Spawn
            PhotonNetwork.InstantiateRoomObject(customerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }

        private int CalculateNumCustomers()
        {
            if (GameStateData.hiddenLevel == 0)
                return 0;

            int randomVariation = Mathf.FloorToInt(GameStateData.hiddenLevel * 1.5f) + Random.Range(0, 2);
            return Mathf.Clamp(randomVariation, 0, maxCustomers);
        }
    }
}
