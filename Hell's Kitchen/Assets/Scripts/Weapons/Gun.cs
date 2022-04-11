using Common.Enums;
using Input;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Gun : MonoBehaviour, IWeapon
    {
        [SerializeField] private float damage  = 50f;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float bulletLifetime = 2f;
    
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Quaternion rotation = Quaternion.identity;

        private InputManager _input => InputManager.Instance;
        private Animator _playerAnimator;

        private void Awake()
        {
            _playerAnimator = GameObject.FindWithTag(Tags.Player).GetComponentInChildren<Animator>();
            
            if (!_playerAnimator)
                throw new MissingReferenceException("Player Animator Not Found");
            
            _input.reference.actions["Attack"].performed += Shoot;
            if(!_input)
                throw new MissingReferenceException("Input Not Found");
        }
    
        private void Start()
        {
            // TODO Remove after testing
            OnEquip();
        }
        
        public void OnEquip()
        {
            Transform hand = GameObject.FindObjectOfType<PlayerController>().CharacterHand;
            if (!hand)
                return;
            
            gameObject.SetActive(true);
            this.transform.parent = hand;
            this.transform.localScale = new Vector3(0.0006f, 0.0006f, 0.0006f);
            this.transform.transform.localPosition = position;
            this.transform.localRotation = rotation;
        }

        public void OnUnequip()
        {
            gameObject.SetActive(false);
        }

        private void Shoot(InputAction.CallbackContext callbackContext)
        {
            _playerAnimator.SetTrigger(PlayerAnimator.Shoot);
        }

        public void Fire()
        {
            Debug.Log("FIRE!");
            var bulletClone = Instantiate(bullet, transform.position, transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            bulletClone.GetComponent<Bullet>().Damage = damage;
            Destroy(bulletClone, bulletLifetime);
        }
    }
}
