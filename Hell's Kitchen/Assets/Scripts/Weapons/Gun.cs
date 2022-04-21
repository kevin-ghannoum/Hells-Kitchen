using System.Collections;
using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Gun : WeaponPickup
    {
        [Header("Parameters")]
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private float bulletLifetime = 2f;
        [SerializeField] private float bulletSpread = 5.0f;
        [SerializeField] private int bulletCount = 1;

        [Header("References")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject muzzlePrefab;
        [SerializeField] private Transform shootPosition;

        [SerializeField] private AudioClip gunshotSound;
        [SerializeField] private float price = 10f;
        public override float Price { get => price; }

        public override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            if (!playerAnimator)
                return;

            playerAnimator.SetTrigger(PlayerAnimator.Shoot);
        }

        private void Fire()
        {
            if (!photonView.IsMine)
                return;

            photonView.RPC(nameof(PlayGunshotSoundRPC), RpcTarget.All);
            ShootBullet();
        }

        [PunRPC]
        private void PlayGunshotSoundRPC()
        {
            AudioSource.PlayClipAtPoint(gunshotSound, transform.position);
        }

        private void ShootBullet()
        {
            var position = shootPosition.position;
            var direction = playerController.AimPoint - position;
            direction.y = 0;
            var playerTransform = playerController.transform;
            var rotation = playerTransform.rotation;
            position.y = playerController.ShootHeight + playerTransform.position.y;
            
            // Bullets
            for (int i = -bulletCount / 2; i <= bulletCount / 2; i++)
            {
                var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, position +  0.5f * i * playerTransform.right, rotation);
                bullet.GetComponent<Bullet>().Damage = Damage;
                bullet.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, i * bulletSpread, 0) * direction.normalized * bulletSpeed;
                StartCoroutine(nameof(DestroyBullet), bullet);
            }

            // Muzzle
            var muzzleFlash = Instantiate(muzzlePrefab, position, rotation);
            Destroy(muzzleFlash, 3.0f);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            var animationEvents = playerAnimator.GetComponentInChildren<AnimationEventIntermediate>();
            animationEvents.fireGun.AddListener(Fire);
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            var animationEvents = playerAnimator.GetComponentInChildren<AnimationEventIntermediate>();
            animationEvents.fireGun.RemoveListener(Fire);
        }

        IEnumerator DestroyBullet(GameObject bullet)
        {
            yield return new WaitForSeconds(bulletLifetime);
            if (bullet != null)
            {
                PhotonNetwork.Destroy(bullet);
            }
        }
    }
}
