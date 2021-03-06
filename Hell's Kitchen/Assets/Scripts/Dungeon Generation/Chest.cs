using Common;
using Common.Enums;
using Input;
using Photon.Pun;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class Chest : Interactable
    {
        [SerializeField] private int amountMinInclusive = 20;
        [SerializeField] private int amountMaxExclusive = 40;
        [SerializeField] private ProximityToggleUI toggleUI;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private AudioClip chestSound;

        private bool _isLooted = false;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private int GetRandomAmountInRange()
        {
            return Random.Range(amountMinInclusive, amountMaxExclusive);
        }

        protected override void Interact()
        {
            if (_isLooted)
                return;
            
            var amount = GetRandomAmountInRange();
            photonView.RPC(nameof(LootChestRPC), RpcTarget.All, amount);
        }
        
        [PunRPC]
        private void LootChestRPC(int amount)
        {
            _isLooted = true;
            toggleUI.IsDisabled = true;
            
            AdrenalinePointsUI.SpawnGoldNumbers(transform.position + 2.0f * Vector3.up, amount);
            AudioSource.PlayClipAtPoint(chestSound, transform.position);
            
            if (photonView.IsMine)
            {
                _animator.SetTrigger(ObjectAnimator.OpenChest);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                GameStateManager.SetCashMoney(GameStateData.cashMoney + amount);
            }
        }
    }
}
