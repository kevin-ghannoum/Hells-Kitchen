using UnityEngine;

namespace Restaurant
{
    public class RestaurantSeat : MonoBehaviour
    {
        public bool IsTaken { get; set; }

        private bool _isSitting;
        public bool IsSitting {
            get => _isSitting;
            set {
                if (value && !_isSitting)
                {
                    table.OnCustomerSit();
                }
                _isSitting = value;
            }
        }

        [SerializeField]
        private RestaurantTable table;

        private void Reset()
        {
            table = GetComponentInParent<RestaurantTable>();
        }
    }
}
