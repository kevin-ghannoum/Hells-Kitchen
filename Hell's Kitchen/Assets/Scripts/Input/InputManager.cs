using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [Header("Input Values")]
        public Vector2 move;
        public bool pickUp;
        public bool run;
        public bool roll;
        public bool attack;
        public bool dropItem;
        public bool throwItem;
        public bool interact;
        public bool openPauseMenu;

        [Header("References")] 
        public PlayerInput reference;

        public static InputManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        public void OnPickUp(InputAction.CallbackContext context)
        {
            pickUp = context.ReadValueAsButton();
        }
    
        public void OnRoll(InputAction.CallbackContext context)
        {
            roll = context.ReadValueAsButton();
        }
    
        public void OnRun(InputAction.CallbackContext context)
        {
            run = context.ReadValueAsButton();
        }
    
        public void OnAttack(InputAction.CallbackContext context)
        {
            attack = context.ReadValueAsButton();
        }
        
        public void OnDropItem(InputAction.CallbackContext context)
        {
            dropItem = context.ReadValueAsButton();
        }

        public void OnThrowItem(InputAction.CallbackContext context)
        {
            throwItem = context.ReadValueAsButton();
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            interact = context.ReadValueAsButton();
        }
        
        public void OnOpenPauseMenu(InputAction.CallbackContext context)
        {
            openPauseMenu = context.ReadValueAsButton();
        }
        
        public void Deactivate()
        {
            reference.DeactivateInput();
        }

        public void Activate()
        {
            reference.ActivateInput();
        }
    }
}
