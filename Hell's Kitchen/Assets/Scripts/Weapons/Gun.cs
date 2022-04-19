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
            Debug.Log($"FIRE - IS MINE - {photonView.IsMine}");
            if (!photonView.IsMine)
                return;
            
            ShootBullet(playerController.transform.right,  playerController.transform.rotation);
        }
        
        private void ShootBullet(Vector3 playerRightTransform, Quaternion playerRotation)
        {
            var position = shootPosition.position;
            var rotation = playerRotation;

            // Bullets
            for (int i = -bulletCount / 2; i <= bulletCount / 2; i++)
            {
                var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, position + playerRightTransform * 0.5f * i, rotation * Quaternion.Euler(0, i * bulletSpread, 0));
                bullet.GetComponent<Bullet>().Damage = Damage;
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
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
