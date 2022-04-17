using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantManager : MonoBehaviour
    {
        public static RestaurantManager Instance;

        [Serializable]
        public class Seat
        {
            public Transform transform;
            public bool taken = false;
        }

        [SerializeField]
        public Seat[] RestaurantSeats;

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

        public Seat FindEmptySeat()
        {
            var availableSeats = RestaurantSeats.Where(s => !s.taken).ToArray();
            if (availableSeats.Length > 0)
            {
                return availableSeats[Random.Range(0, availableSeats.Length)];
            }
            return null;
        }

    }
}
