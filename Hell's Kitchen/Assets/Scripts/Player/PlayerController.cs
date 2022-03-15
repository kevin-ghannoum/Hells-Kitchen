using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;
        [SerializeField] private Transform camera;

        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float turnSmoothVelocity;
        [SerializeField] private float acceleration = 2f;

        private float speed = 6f;
    
        private static InputManager Input => InputManager.Instance;

        private void Start()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        private void Awake()
        {
            Input.reference.FindAction("Roll").performed += Roll;
            Input.reference.FindAction("Attack").performed += Attack;
        }

        private void Update()
        {
            MovePlayer();
        }

        void Attack(InputAction.CallbackContext callbackContext)
        {
        
        }

        void Roll(InputAction.CallbackContext callbackContext)
        {
            animator.SetTrigger("Roll"); 
        }

        private void MovePlayer()
        {
            float  targetSpeed = Input.move.normalized.magnitude * GetMovementSpeed();
            speed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime);
            Vector3 movement = Vector3.forward * speed * Time.deltaTime;
            characterController.Move(transform.TransformDirection(movement));
        
            animator.SetFloat("Speed", speed);
        }

        private float GetMovementSpeed()
        {
            return Input.run ? runSpeed : walkSpeed;
        }
   
    }
}
