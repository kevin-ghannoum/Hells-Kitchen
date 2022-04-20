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

        [Header("Stamina")]
        [SerializeField] private float staminaCostRun = 1.0f;
        [SerializeField] private float staminaCostRoll = 1.0f;
        [SerializeField] private float staminaRegenRate = 1.0f;

        [Header("Hand")]
        [SerializeField] public Transform CharacterHand;
        [SerializeField] public Transform shootHeight;

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
        }
        
        private void Update()
        {
            if (!_photonView.IsMine) return;

            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Move))
            {
                rollStartup = true;
                MovePlayer();
                RotatePlayer();
            }
            else if (animatorStateInfo.IsName(PlayerAnimator.Shoot)) {
                RotatePlayer();
            }
            else if (animatorStateInfo.IsName(PlayerAnimator.Roll))
            {
                if (rollStartup)
                    transform.rotation = Quaternion.LookRotation(new Vector3(_input.move.x, 0, _input.move.y));
                else
                    RotatePlayer();
                rollStartup = false;
                float rollSpeed = rollSpeedCurve.Evaluate(animatorStateInfo.normalizedTime) * (runSpeed - walkSpeed) + walkSpeed;
                Vector3 movement = Vector3.forward * rollSpeed * Time.deltaTime;
                movement = Vector3.forward * rollSpeed * Time.deltaTime;
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
            float turnSmoothV = turnSmoothVelocity;
            Vector3 targetDirection = new Vector3(_input.move.x, 0f, _input.move.y);
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.Shoot)) {
                targetDirection = clickTarget - transform.position;
                turnSmoothV*= 1.5f;
            }
            else if (targetDirection == Vector3.zero)
                return;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSmoothV * Time.deltaTime);
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

        Vector3 clickTarget = Vector3.zero;
        public void FaceTarget(Vector3 target)
        {
            if (!_animator)
                return;

            clickTarget = target;
            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            //if (!animatorStateInfo.IsName(PlayerAnimator.Roll) && (animatorStateInfo.IsName(PlayerAnimator.Move) || animatorStateInfo.normalizedTime > 0.5f))
            if (!animatorStateInfo.IsName(PlayerAnimator.Roll))
            {
                //transform.rotation = Quaternion.LookRotation(target - transform.position);
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
