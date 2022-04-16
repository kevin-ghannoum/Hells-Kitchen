using System;
using System.Collections.Generic;
using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;
using PlayerInventory;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance; // singleton
        private InputManager _input => InputManager.Instance;

        [Header("Parameters")]
        [SerializeField] private float runSpeed = 15f;
        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float turnSmoothVelocity = 10f;
        [SerializeField] private float speedSmoothVelocity = 10f;
        [SerializeField] private AnimationCurve rollSpeedCurve;
        [SerializeField] private InventoryUI _inventoryUI;
       
        [Header("Stamina")]
        [SerializeField] private float staminaCostRun = 1.0f;
        [SerializeField] private float staminaCostRoll = 1.0f;
        [SerializeField] private float staminaRegenRate = 1.0f;

        [Header("Hand")]
        [SerializeField] public Transform CharacterHand;

        [Header("Melee Attack")]
        [SerializeField] public Transform DamagePosition;
        [SerializeField] public float DamageRadius = 1.5f;

        private Animator _animator;
        private CharacterController _characterController;
        private Inventory _inventory = new Inventory();
        private float _speed = 0f;
        private IPickup _currentPickup;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
            _inventory = new Inventory();
            
            _input.reference.actions["Roll"].performed += Roll;
            _input.reference.actions["PickUp"].performed += PickUp;
        }

        private void OnDestroy()
        {
            if (!_input)
                return;
            
            _input.reference.actions["Roll"].performed -= Roll;
            _input.reference.actions["PickUp"].performed -= PickUp;
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
                float rollSpeed = rollSpeedCurve.Evaluate(animatorStateInfo.normalizedTime) * (runSpeed - walkSpeed) + walkSpeed;
                Vector3 movement = Vector3.forward * rollSpeed * Time.deltaTime;
                _characterController.Move(transform.TransformDirection(movement));
                _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
            }
            else
            {
                _characterController.Move(Vector3.zero);
                _speed = 0;
                _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
            }

            UpdateStamina();
        }

        public void OnPickupTriggerEnter(IPickup pickup)
        {
            _currentPickup = pickup;
        }

        public void OnPickupTriggerExit(IPickup pickup)
        {
            if (_currentPickup == pickup)
            {
                _currentPickup = null;
            }
        }

        #region PlayerActions

        public void Roll(InputAction.CallbackContext callbackContext)
        {
            if (GameStateManager.Instance.playerCurrentStamina > staminaCostRoll)
            {
                GameStateManager.Instance.playerCurrentStamina -= staminaCostRoll;
                _animator.SetTrigger(PlayerAnimator.Roll);
            }
        }

        public void PickUp(InputAction.CallbackContext callbackContext)
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
            bool canSprint = CanSprint();
            return _input.run && canSprint ? runSpeed : walkSpeed;
        }

        private bool CanSprint()
        {
            return GameStateManager.Instance.playerCurrentStamina > 0;
        }

        private void UpdateStamina()
        {
            var stamina = GameStateManager.Instance.playerCurrentStamina;
            if (_input.run)
            {
                stamina -= Time.deltaTime * staminaCostRun;
                if (stamina < 0)
                    stamina = 0;
            }
            else if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.Roll))
            {
                stamina += Time.deltaTime * staminaRegenRate;
                if (stamina > GameStateManager.Instance.playerMaxStamina)
                    stamina = GameStateManager.Instance.playerMaxStamina;
            }
            GameStateManager.Instance.playerCurrentStamina = stamina;
        }

        #endregion

        #region PlayerInventory

        public Inventory GetPlayerInventory()
        {
            return _inventory;
        }

        public void AddItemToInventory(Item item, int quantity)
        {
            _inventory.AddItemToInventory(item, quantity);
            _inventoryUI.UpdateInventory(_inventory.GetInventoryItems());
        }

        public void RemoveItemFromInventory(Item item, int quantity)
        {
            _inventory.RemoveItemFromInventory(item, quantity);
            _inventoryUI.UpdateInventory(_inventory.GetInventoryItems());
        }

        #endregion

        #region PlayerSpaceActions

        public void FaceTarget(Vector3 target)
        {
            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (!animatorStateInfo.IsName(PlayerAnimator.Roll) &&
                (animatorStateInfo.IsName(PlayerAnimator.Move) || animatorStateInfo.normalizedTime > 0.5f))
            {
                transform.rotation = Quaternion.LookRotation(target - transform.position);
            }
        }

        public void InflictMeleeDamage()
        {
            float damage = GetComponentInChildren<WeaponPickup>()?.Damage ?? 0.0f;
            var colliders = Physics.OverlapSphere(DamagePosition.position, DamageRadius, ~(1 << Layers.Player));
            foreach (var col in colliders)
            {
                col.gameObject.GetComponent<IKillable>()?.TakeDamage(damage);
            }
        }

        public void ExecutePickUp()
        {
            _currentPickup?.PickUp();
        }

        #endregion
    }
}
