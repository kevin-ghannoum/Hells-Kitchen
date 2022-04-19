using Photon.Pun;
using UnityEngine;

namespace Restaurant
{
    public class RestaurantSeat : MonoBehaviour, IPunObservable
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
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsTaken);
                stream.SendNext(IsSitting);
            } 
            else if (stream.IsReading)
            {
                IsTaken = (bool) stream.ReceiveNext();
                _isSitting = (bool)stream.ReceiveNext();
            }
        }
    }
}
