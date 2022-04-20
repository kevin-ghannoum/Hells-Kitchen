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
        [SerializeField] public Transform shootPosition;
        
        [SerializeField] private float price =  10f;
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
            var direction = base.aimPoint - shootPosition.position;
            direction.y = 0;
            var position = shootPosition.position;
            var rotation = playerController.transform.rotation;
            Vector3 initialPosition = shootPosition.position;
            initialPosition.y = playerController.shootHeight.position.y;
            // Bullets
            for (int i = -bulletCount / 2; i <= bulletCount / 2; i++)
            {
                var bullet = Instantiate(bulletPrefab, initialPosition + playerController.transform.right * 0.5f * i, rotation * Quaternion.Euler(0, i * bulletSpread, 0));
                bullet.GetComponent<Bullet>().Damage = Damage;
                bullet.GetComponent<Rigidbody>().velocity = direction.normalized  * bulletSpeed;
                Destroy(bullet, bulletLifetime);
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
    }
}
