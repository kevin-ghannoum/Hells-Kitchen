using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;

        [SerializeField] private float runSpeed = 15f;
        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float turnSmoothVelocity = 10f;
        [SerializeField] private float speedSmoothVelocity = 10f;
        private float speed = 0f;
        
        private InputManager _input => InputManager.Instance;

        private void Start()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        private void Awake()
        {
            _input.reference.actions["Roll"].performed += Roll;
            _input.reference.actions["Attack"].performed += Attack;
            _input.reference.actions["PickUp"].performed += PickUp;
        }

        private void Update()
        {
            MovePlayer();
            RotatePlayer();
        }

        void Attack(InputAction.CallbackContext callbackContext)
        {
            animator.SetTrigger(PlayerAnimator.SwordAttack);
        }

        void Roll(InputAction.CallbackContext callbackContext)
        {
            animator.SetTrigger(PlayerAnimator.Roll); 
        }

        void PickUp(InputAction.CallbackContext callbackContext)
        {
            animator.SetTrigger(PlayerAnimator.PickUp);
        }

        private void MovePlayer()
        {
            float  targetSpeed = _input.move.normalized.magnitude * GetMovementSpeed();
            speed = Mathf.Lerp(speed, targetSpeed, speedSmoothVelocity * Time.deltaTime);
            Vector3 movement = Vector3.forward * speed * Time.deltaTime;
            characterController.Move(transform.TransformDirection(movement));
        
            animator.SetFloat(PlayerAnimator.Speed, speed);
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
    }
}