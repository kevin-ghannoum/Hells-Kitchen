using Common;
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
        [SerializeField] private Transform shootPosition;
        [SerializeField] private float bulletLifetime = 2f;
    
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Quaternion rotation = Quaternion.identity;

        private InputManager _input => InputManager.Instance;
        
        private Animator _playerAnimator;
        private GameObject _player;
        private bool _canBePickedUp = false;

        private void Awake()
        {
            _player = GameObject.FindWithTag(Tags.Player);
            if (!_player)
                return;
            
            _playerAnimator = _player.GetComponentInChildren<Animator>();
            if (!_playerAnimator)
                throw new MissingReferenceException("Player Animator Not Found");
        }
        
        public void PickUpItem()
        {
            AddListeners();
            ReparentObject();
            GameStateManager.Instance.carriedWeapon = gameObject;
        }

        public void Unequip(InputAction.CallbackContext callbackContext)
        {
            _canBePickedUp = true;
            transform.parent = null;
            GameStateManager.Instance.carriedWeapon = null;
            RemoveListeners();
        }

        private void Shoot(InputAction.CallbackContext callbackContext)
        {
            _playerAnimator.SetTrigger(PlayerAnimator.Shoot);
        }

        public void Fire()
        {
            var bulletClone = Instantiate(bullet, shootPosition.position, _player.transform.rotation);
            bulletClone.GetComponent<Bullet>().Damage = damage;
            bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.transform.forward * bulletSpeed;
            Destroy(bulletClone, bulletLifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            _canBePickedUp = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _canBePickedUp = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_canBePickedUp && !GameStateManager.Instance.IsCarryingWeapon && _input.pickUp)
            {
                PickUpItem();
                _canBePickedUp = false;
            }
        }

        private void AddListeners()
        {
            if(!_input)
                throw new MissingReferenceException("Input Not Found");
            
            _input.reference.actions["Attack"].performed += Shoot;
            _input.reference.actions["DropItem"].performed += Unequip;
            
            var animationEvents = _player.GetComponentInChildren<AnimationEventIntermediate>();
            animationEvents.shootGun.AddListener(Fire);
        }

        private void RemoveListeners()
        {
            _input.reference.actions["Attack"].performed -= Shoot;
            _input.reference.actions["DropItem"].performed -= Unequip;
            
            var animationEvents = _player.GetComponentInChildren<AnimationEventIntermediate>();
            animationEvents.shootGun.RemoveListener(Fire);
        }

        private void ReparentObject()
        {
            Transform hand = _player.GetComponent<PlayerController>().CharacterHand;
            gameObject.transform.SetParent(hand, false);
            gameObject.transform.localScale = new Vector3(0.0006f, 0.0006f, 0.0006f);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
        }
    }
}
