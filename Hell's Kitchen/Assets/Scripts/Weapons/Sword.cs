using System;
using Common;
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
            ReparentObject();
            AddListeners();
            GameStateManager.Instance.carriedWeapon = gameObject;
        }
    
        public void Unequip(InputAction.CallbackContext callbackContext)
        {
            _canBePickedUp = true;
            transform.parent = null;
            RemoveListeners();
            GameStateManager.Instance.carriedWeapon = null;
        }

        void Attack(InputAction.CallbackContext callbackContext)
        {
            _playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }

        private void ReparentObject()
        {
            Transform hand = GameObject.FindObjectOfType<PlayerController>().CharacterHand;
            if (!hand)
                return;
            
            gameObject.transform.SetParent(hand, false);
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
        }

        private void AddListeners()
        {
            if(!_input)
                throw new MissingReferenceException("Input Not Found");
            
            _input.reference.actions["Attack"].performed += Attack;
            _input.reference.actions["DropItem"].performed += Unequip;
        }

        private void RemoveListeners()
        {
            _input.reference.actions["Attack"].performed -= Attack;
            _input.reference.actions["DropItem"].performed -= Unequip;
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
    }
}

