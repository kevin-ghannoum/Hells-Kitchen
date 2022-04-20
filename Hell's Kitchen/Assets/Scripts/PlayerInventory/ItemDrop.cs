using Common;
using Common.Enums;
using Enums.Items;
using Photon.Pun;
using UI;
using UnityEngine;

namespace PlayerInventory
{
    public class ItemDrop : MonoBehaviour, IPunObservable
    {
        [SerializeField]
        public ItemInstance item;
        
        [SerializeField]
        public int quantity = 1;

        [SerializeField]
        private PhotonView photonView;

        private void OnTriggerEnter(Collider other)
        {
            // items can be picked up by both the chef (player) and sous-chef
            if (other.CompareTag(Tags.Player) || other.CompareTag(Tags.SousChef))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    photonView.RPC(nameof(PickUp), RpcTarget.All);
                }
            }
        }

        [PunRPC]
        private void PickUp()
        {
            AdrenalinePointsUI.SpawnIngredientString(transform.position + 2.0f * Vector3.up, "+ " + quantity + " " + item);
            if (photonView.IsMine)
            {
                GameStateManager.AddItemToInventory(item, quantity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)item);
                stream.SendNext(quantity);
            }
            else if (stream.IsReading)
            {
                item = (ItemInstance)stream.ReceiveNext();
                quantity = (int)stream.ReceiveNext();
            }
        }
    }
}
