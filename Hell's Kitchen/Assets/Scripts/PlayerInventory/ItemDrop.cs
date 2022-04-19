using Common;
using Common.Enums;
using Enums.Items;
using Photon.Pun;
using UI;
using UnityEngine;

namespace PlayerInventory
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField]
        private int quantity = 1;

        [SerializeField]
        private ItemInstance item;

        [SerializeField]
        private PhotonView photonView;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
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
    }
}
