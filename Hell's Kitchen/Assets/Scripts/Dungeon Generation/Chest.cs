using Common;
using Common.Enums;
using Input;
using Photon.Pun;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private int amountMinInclusive = 20;
        [SerializeField] private int amountMaxExclusive = 40;
        [SerializeField] private ProximityToggleUI toggleUI;
        [SerializeField] private PhotonView photonView;

        private bool _isLooted = false;
        
        private InputManager _input => InputManager.Instance;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private int GetRandomAmountInRange()
        {
            return Random.Range(amountMinInclusive, amountMaxExclusive);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_isLooted && other.gameObject.CompareTag(Tags.Player))
            {
                var pv = other.gameObject.GetComponent<PhotonView>();
                if (pv.IsMine && _input.interact)
                {
                    _isLooted = true;
                    _animator.SetTrigger(ObjectAnimator.OpenChest);
                    
                    var amount = GetRandomAmountInRange();
                    photonView.RPC(nameof(LootChest), RpcTarget.All, amount);
                }
            }
        }

        [PunRPC]
        private void LootChest(int amount)
        {
            _isLooted = true;
            toggleUI.IsDisabled = true;
            AdrenalinePointsUI.SpawnGoldNumbers(transform.position + 2.0f * Vector3.up, amount);
            if (PhotonNetwork.IsMasterClient)
            {
                GameStateManager.SetCashMoney(GameStateData.cashMoney + amount);
            }
        }
    }
}
