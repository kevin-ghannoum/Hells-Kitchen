using System;
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
        
        [SerializeField]
        private int numCustomersToSpawn = 5;

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
            for (int i = 0; i < numCustomersToSpawn; i++)
            {
                Invoke(nameof(SpawnCustomer), spawnDelay * i + Random.Range(-0.5f, 0.5f));
            }
        }

        private void SpawnCustomer()
        {
            // Random spawn point
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Random customer prefab
            var customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];

            // Spawn
            Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        }

    }
}
