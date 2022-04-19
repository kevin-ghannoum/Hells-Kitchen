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
        [SerializeField] private AudioClip swordSlashSound;
        public override float Price { get => price; }
        public override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            if (!playerAnimator)
                return;

            SwordAttack();
        }

        private void SwordAttack()
        {
            if (photonView.IsMine && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.SwordAttack))
            {
                photonView.RPC(nameof(PlaySWordSlashSoundRPC), RpcTarget.All);
            }
            playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }

        [PunRPC]
        private void PlaySWordSlashSoundRPC()
        {
            AudioSource.PlayClipAtPoint(swordSlashSound, transform.position);
        }
    }
}

