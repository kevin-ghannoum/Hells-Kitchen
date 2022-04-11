using System;
using Common.Enums;
using Common.Interfaces;
using Input;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Sword : MonoBehaviour, IWeapon
    {
        [SerializeField] private float damage = 10f;

        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Quaternion rotation = Quaternion.identity;

        private InputManager _input => InputManager.Instance;
        private Animator _playerAnimator;

        private void Awake()
        {
            _playerAnimator = GameObject.FindWithTag(Tags.Player).GetComponentInChildren<Animator>();

            if (!_playerAnimator)
                throw new MissingReferenceException("Player Animator Not Found");
            
            _input.reference.actions["Attack"].performed += Attack;
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
            this.transform.parent.transform.parent = hand;
            this.transform.parent.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            this.transform.parent.transform.localPosition = position;
            this.transform.localRotation = rotation;
        }
    
        public void OnUnequip()
        {
            gameObject.SetActive(false);
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            var obj = collision.gameObject;
            if (_input.attack && obj.TryGetComponent(out IKillable killable))
            {
                killable.TakeDamage(damage);
            }
        }
        
        void Attack(InputAction.CallbackContext callbackContext)
        {
            _playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }
    }
}

