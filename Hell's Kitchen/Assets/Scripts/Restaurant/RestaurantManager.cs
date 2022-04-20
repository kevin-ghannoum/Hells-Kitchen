using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Restaurant
{
    public class RestaurantManager : MonoBehaviour
    {
        public static RestaurantManager Instance;

        [SerializeField]
        public RestaurantSeat[] Seats;

        [SerializeField]
        public RestaurantTable[] Tables;
        
        [SerializeField, Range(10,20)] private int minOrderPrice = 20;
        [SerializeField, Range(20,30)] private int maxOrderPrice = 25;

        public int MaxOrderPrice { get => maxOrderPrice; }

        public int MinOrderPrice { get => minOrderPrice; }

        public List<RestaurantOrder> OrderList => Instance.Tables.Aggregate(
            new List<RestaurantOrder>(), 
            (acc, t) => {
                acc.AddRange(t.OrderList);
                return acc;
            });

        private void Reset()
        {
           Seats = FindObjectsOfType<RestaurantSeat>();
           Tables = FindObjectsOfType<RestaurantTable>();
        }

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

        public RestaurantSeat FindEmptySeat()
        {
            var availableSeats = Seats.Where(s => !s.IsTaken).ToArray();
            if (availableSeats.Length > 0)
            {
                return availableSeats[Random.Range(0, availableSeats.Length)];
            }
            return null;
        }

    }
}
