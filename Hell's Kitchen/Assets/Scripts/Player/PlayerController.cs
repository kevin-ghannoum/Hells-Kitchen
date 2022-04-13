using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance; // singleton

        [Header("Parameters")]
        [SerializeField] private float runSpeed = 15f;
        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float turnSmoothVelocity = 10f;
        [SerializeField] private float speedSmoothVelocity = 10f;
        [SerializeField] private AnimationCurve rollSpeedCurve;

        [Header("Hand")]
        [SerializeField] public Transform CharacterHand;

        [Header("Melee Attack")]
        [SerializeField] public Transform DamagePosition;
        [SerializeField] public float DamageRadius = 1.5f;

        private Animator _animator;
        private CharacterController _characterController;
        private Inventory _inventory;
        private float _speed = 0f;

        private InputManager _input => InputManager.Instance;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
            _inventory = new Inventory();
        }

        private void Awake()
        {
            // Singleton instance
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _input.reference.actions["Roll"].performed += Roll;

            _input.reference.actions["PickUp"].performed += PickUp;
        }

        private void Update()
        {
            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Move))
            {
                MovePlayer();
                RotatePlayer();
            }
            else if (animatorStateInfo.IsName(PlayerAnimator.Roll))
            {
                float speed = rollSpeedCurve.Evaluate(animatorStateInfo.normalizedTime) * (runSpeed - walkSpeed) + walkSpeed;
                Vector3 movement = Vector3.forward * speed * Time.deltaTime;
                _characterController.Move(transform.TransformDirection(movement));
                _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
            }
            else
            {
                _characterController.Move(Vector3.zero);
                _speed = 0;
                _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
            }
        }

        #region PlayerActions

        void Roll(InputAction.CallbackContext callbackContext)
        {
            _animator.SetTrigger(PlayerAnimator.Roll);
        }

        void PickUp(InputAction.CallbackContext callbackContext)
        {
            _animator.SetTrigger(PlayerAnimator.PickUp);
        }

        #endregion

        #region PlayerMovement

        private void MovePlayer()
        {
            float targetSpeed = _input.move.normalized.magnitude * GetMovementSpeed();
            _speed = Mathf.Lerp(_speed, targetSpeed, speedSmoothVelocity * Time.deltaTime);
            Vector3 movement = Vector3.forward * _speed * Time.deltaTime;
            _characterController.Move(transform.TransformDirection(movement));
            _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
        }

        private void RotatePlayer()
        {
            Vector3 targetDirection = new Vector3(_input.move.x, 0f, _input.move.y);
            if (targetDirection == Vector3.zero)
                return;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSmoothVelocity * Time.deltaTime);
        }

        private float GetMovementSpeed()
        {
            return _input.run ? runSpeed : walkSpeed;
        }

        #endregion

        #region PlayerInventory

        public Dictionary<Item, int> GetPlayerInventory()
        {
            return _inventory.GetInventory();
        }

        public void AddItemToInventory(Item item, int quantity)
        {
            _inventory.AddItemToInventory(item, quantity);
        }

        public void RemoveItemFromInventory(Item item, int quantity)
        {
            _inventory.RemoveItemFromInventory(item, quantity);
        }

        public void FaceDirection(Vector3 direction)
        {
            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (!animatorStateInfo.IsName(PlayerAnimator.Roll) &&
                (animatorStateInfo.IsName(PlayerAnimator.Move) || animatorStateInfo.normalizedTime > 0.5f))
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        public void InflictMeleeDamage()
        {
            float damage = GameStateManager.Instance.carriedWeapon?.GetComponent<WeaponPickup>()?.damage ?? 0.0f;
            var colliders = Physics.OverlapSphere(DamagePosition.position, DamageRadius, ~(1 << Layers.Player));
            foreach (var col in colliders)
            {
                col.gameObject.GetComponent<IKillable>()?.TakeDamage(damage);
            }
        }

        #endregion
    }
}
