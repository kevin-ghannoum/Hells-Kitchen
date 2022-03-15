using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool pickUp;
    public bool run;
    public bool roll;
    public bool attack;

    [Header("References")] 
    public Input reference;

    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(Instance.gameObject);
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

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
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
    
}
