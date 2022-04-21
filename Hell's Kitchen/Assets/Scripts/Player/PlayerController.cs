using System.Linq;
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
        [SerializeField] public float ShootHeight = 1.7f;

        [Header("Stamina")]
        [SerializeField] private float staminaCostRun = 1.0f;
        [SerializeField] private float staminaCostRoll = 1.0f;
        [SerializeField] private float staminaRegenRate = 1.0f;

        [Header("Transform References")]
        [SerializeField] public Transform CharacterHand;

        [Header("Melee Attack")]
        [SerializeField] public Transform DamagePosition;
        [SerializeField] public float DamageRadius = 1.5f;

        [SerializeField] private PhotonView _photonView;

        private Animator _animator;
        private CharacterController _characterController;

        private float _speed = 0f;
        private IPickup _currentPickup;
        
        bool _rollStartup = true;

        public Vector3 AimPoint { get; set; }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
        }
        
        private void Update()
        {
            if (!_photonView.IsMine) return;

            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Move))
            {
                _rollStartup = true;
                MovePlayer();
                RotatePlayer();
            }
            else if (animatorStateInfo.IsName(PlayerAnimator.Roll))
            {
                //this line allows to roll backwards or sideways instantly without having to turn first, good for kiting
                if (_rollStartup)   
                    transform.rotation = Quaternion.LookRotation(new Vector3(_input.move.x, 0, _input.move.y));
           
                _rollStartup = false;
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

            if (InputManager.Actions.Roll.triggered)
                Roll();

            if (InputManager.Actions.PickUp.triggered)
                PickUp();

            UpdateStamina();
        }

        public void OnPickupTriggerStay(IPickup pickup)
        {
            _currentPickup = pickup;
        }

        #region PlayerActions

        private void Roll()
        {
            if (GameStateData.playerCurrentStamina > staminaCostRoll)
            {
                GameStateData.playerCurrentStamina -= staminaCostRoll;
                _animator.SetTrigger(PlayerAnimator.Roll);
            }
        }

        private void PickUp()
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
            _animator.SetFloat(PlayerAnimator.Speed, _speed / runSpeed);
        }

        private void RotatePlayer()
        {
            Vector3 targetDirection = new Vector3(_input.move.x, 0f, _input.move.y);
            if (targetDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSmoothVelocity * Time.deltaTime);
            }
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
            if (!animatorStateInfo.IsName(PlayerAnimator.Roll) && (animatorStateInfo.IsName(PlayerAnimator.Move) || animatorStateInfo.normalizedTime > 0.5f))
            {
                AimPoint = target;
                transform.rotation = Quaternion.LookRotation(target - transform.position);
            }
        }

        public void InflictMeleeDamage()
        {
            if (!_photonView.IsMine)
                return;

            float damage = GetComponentInChildren<WeaponPickup>()?.Damage ?? 0.0f;
            var colliders = Physics.OverlapSphere(DamagePosition.position, DamageRadius, ~(1 << Layers.Player))
                .Where(c => c.CompareTag(Tags.Enemy));
            foreach (var col in colliders)
            {
                col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, damage);
            }
        }

        public void ExecutePickUp()
        {
            _currentPickup?.PickUp();
        }

        #endregion
    }
}
