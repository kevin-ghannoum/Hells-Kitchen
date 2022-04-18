using Common.Interfaces;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Weapons
{
    public class Sword : WeaponPickup, IWeapon
    {
        [SerializeField] private float price = 10f;
        public override float Price { get => price; }
        public override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            if (!playerAnimator)
                return;

            var photonView = GetComponentInParent<PhotonView>();
            photonView.RPC("SwordAttackRPC", RpcTarget.AllViaServer);
        }

        [PunRPC]
        private void SwordAttackRPC()
        {
            playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }
    }
}

