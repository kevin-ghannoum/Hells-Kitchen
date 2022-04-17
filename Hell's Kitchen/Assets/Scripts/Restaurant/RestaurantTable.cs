using System.Linq;
using UI;
using UnityEngine;

namespace Restaurant
{
    public class RestaurantTable : MonoBehaviour
    {
        [SerializeField]
        private RestaurantSeat[] seats;

        [SerializeField]
        private ToggleUI toggleUI;

        private void Reset()
        {
            seats = GetComponentsInChildren<RestaurantSeat>();
        }

        private void Update()
        {
            toggleUI.IsDisabled = !seats.Any(s => s.IsSitting);
        }
    }
}
