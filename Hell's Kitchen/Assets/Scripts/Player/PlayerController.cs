using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

        [Header("Parameters")]
        [SerializeField] private float runSpeed = 15f;
        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float turnSmoothVelocity = 10f;
        [SerializeField] private float speedSmoothVelocity = 10f;
        [SerializeField] private AnimationCurve rollSpeedCurve;

        [Header("Stamina")]
        [SerializeField] private float staminaCostRun = 1.0f;
        [SerializeField] private float staminaCostRoll = 1.0f;
        [SerializeField] private float staminaRegenRate = 1.0f;

        [Header("Hand")]
        [SerializeField] public Transform CharacterHand;

        [Header("Melee Attack")]
        [SerializeField] public Transform DamagePosition;
        [SerializeField] public float DamageRadius = 1.5f;

        [SerializeField] private PhotonView _photonView;

        private Animator _animator;
        private CharacterController _characterController;


        private float _speed = 0f;
        private IPickup _currentPickup;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();

            if (_photonView.IsMine)
            {
                _input.reference.actions["Roll"].performed += Roll;
                _input.reference.actions["PickUp"].performed += PickUp;
            }
        }

        private void OnDestroy()
        {
            if (_photonView.IsMine && _input)
            {
                _input.reference.actions["Roll"].performed -= Roll;
                _input.reference.actions["PickUp"].performed -= PickUp;
            }
        }

        private void Update()
        {
            if (!_photonView.IsMine) return;

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
            if (GameStateData.playerCurrentStamina > staminaCostRoll)
            {
                GameStateData.playerCurrentStamina -= staminaCostRoll;
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
            return GameStateData.playerCurrentStamina > 0;
        }

        private void UpdateStamina()
        {
            var stamina = GameStateData.playerCurrentStamina;
            if (_input.run)
            {
                stamina -= Time.deltaTime * staminaCostRun;
                if (stamina < 0)
                    stamina = 0;
            }
            else if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.Roll))
            {
                stamina += Time.deltaTime * staminaRegenRate;
                if (stamina > GameStateData.playerMaxStamina)
                    stamina = GameStateData.playerMaxStamina;
            }
            GameStateData.playerCurrentStamina = stamina;
        }

        #endregion

        #region PlayerSpaceActions

        public void FaceTarget(Vector3 target)
        {
            if (!_animator)
                return;

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
