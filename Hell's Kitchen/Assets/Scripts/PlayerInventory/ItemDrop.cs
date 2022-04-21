using Common;
using Common.Enums;
using Enums.Items;
using Photon.Pun;
using UI;
using UnityEngine;
using System.Collections;

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

        private void OnTriggerStay(Collider other)
        {
            // items can be picked up by both the chef (player) and sous-chef
            if (other.CompareTag(Tags.Player))
            {
                photonView.RPC(nameof(PickUpRPC), RpcTarget.All);
            }
            else if (other.CompareTag(Tags.SousChef))
            {
                // check if the sous chef is in loot state and makes sure sous chef is picking up this item
                if (other.TryGetComponent(out HealerStateManager healerStateManager))
                {
                    if (healerStateManager.IsInLootState() && ReferenceEquals(other.GetComponent<SousChef>().targetLoot.gameObject, gameObject))
                    {
                        StartCoroutine(PickUpBySousChef(other));
                    }
                }
                else if (other.TryGetComponent(out KnightStateManager knightStateManager))
                {
                    if (knightStateManager.IsInLootState() && ReferenceEquals(other.GetComponent<SousChef>().targetLoot.gameObject, gameObject))
                    {
                        StartCoroutine(PickUpBySousChef(other));
                    }
                }
            }
        }

        [PunRPC]
        private void PickUpRPC()
        {
            AdrenalinePointsUI.SpawnIngredientString(transform.position + 2.0f * Vector3.up, "+ " + quantity + " " + item);
            if (photonView.IsMine)
            {
                GameStateManager.AddItemToInventory(item, quantity);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        IEnumerator PickUpBySousChef(Collider other)
        {
            // wait untill the sous chef pick up animation ends
            yield return new WaitForSeconds(1.5f);
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                photonView.RPC(nameof(PickUpRPC), RpcTarget.All);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int) item);
                stream.SendNext(quantity);
            }
            else if (stream.IsReading)
            {
                item = (ItemInstance) stream.ReceiveNext();
                quantity = (int) stream.ReceiveNext();
            }
        }
    }
}
