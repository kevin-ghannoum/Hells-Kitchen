//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputSystem/Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputSystem
{
    public partial class @PlayerInput : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""8ad0b397-9893-4523-98bd-b315cee0fe71"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""62bdaae1-5d1f-473f-a439-bd890e923c48"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PickUp"",
                    ""type"": ""Button"",
                    ""id"": ""94c69b95-9613-4b4d-99f1-797d202b9d98"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""8ef716dc-42a9-4c44-8667-347dd5dcfcf2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""f01eae4f-0ab0-411e-8760-c48b74bffc61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""1b503966-d30e-4d87-952a-7a57563f1444"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropItem"",
                    ""type"": ""Button"",
                    ""id"": ""0cf23d2b-bed0-427c-a481-b4f7c0bc0e7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThrowItem"",
                    ""type"": ""Button"",
                    ""id"": ""25cafb69-30d3-4f0d-9a38-7f6f7e561399"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f4e55406-7dba-4539-a31a-1ffc02464442"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenPauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""b48f71ea-8962-4591-8416-4531aa899be7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e718712f-f895-4285-8c32-7209f99f0290"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""5ff5e9a6-e2d1-469d-88bb-5a2d1ad0b32f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""2a86017a-c016-4f39-bb84-e7812757da57"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""d35026f8-5cde-48dc-92ec-4d3097b3ad97"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""763ccca0-20a9-4365-a441-d0b8946b37cc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bf1740cd-ab87-44dd-8266-de3e0520936b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d221342-04e2-4cf1-9111-5b40f11ce05d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4723c2bc-b73c-43ef-b70e-0fb916b9dedf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36ed2af1-cd20-408c-9645-21bfd3f53aee"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9901cf8-9356-4e43-8429-3e656b6eae90"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee2da1dc-4044-43b7-a9fe-0775c665a190"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9d44189-fda1-4bab-ab0c-43554aed7d6d"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb9d539f-6e8e-4389-b839-8e126aa25bb4"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenPauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_PickUp = m_Player.FindAction("PickUp", throwIfNotFound: true);
            m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
            m_Player_Roll = m_Player.FindAction("Roll", throwIfNotFound: true);
            m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
            m_Player_DropItem = m_Player.FindAction("DropItem", throwIfNotFound: true);
            m_Player_ThrowItem = m_Player.FindAction("ThrowItem", throwIfNotFound: true);
            m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
            m_Player_OpenPauseMenu = m_Player.FindAction("OpenPauseMenu", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_PickUp;
        private readonly InputAction m_Player_Attack;
        private readonly InputAction m_Player_Roll;
        private readonly InputAction m_Player_Run;
        private readonly InputAction m_Player_DropItem;
        private readonly InputAction m_Player_ThrowItem;
        private readonly InputAction m_Player_Interact;
        private readonly InputAction m_Player_OpenPauseMenu;
        public struct PlayerActions
        {
            private @PlayerInput m_Wrapper;
            public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @PickUp => m_Wrapper.m_Player_PickUp;
            public InputAction @Attack => m_Wrapper.m_Player_Attack;
            public InputAction @Roll => m_Wrapper.m_Player_Roll;
            public InputAction @Run => m_Wrapper.m_Player_Run;
            public InputAction @DropItem => m_Wrapper.m_Player_DropItem;
            public InputAction @ThrowItem => m_Wrapper.m_Player_ThrowItem;
            public InputAction @Interact => m_Wrapper.m_Player_Interact;
            public InputAction @OpenPauseMenu => m_Wrapper.m_Player_OpenPauseMenu;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @PickUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @PickUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @PickUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                    @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                    @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                    @Roll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                    @Roll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                    @Roll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                    @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                    @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                    @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                    @DropItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropItem;
                    @DropItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropItem;
                    @DropItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropItem;
                    @ThrowItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowItem;
                    @ThrowItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowItem;
                    @ThrowItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowItem;
                    @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                    @OpenPauseMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenPauseMenu;
                    @OpenPauseMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenPauseMenu;
                    @OpenPauseMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenPauseMenu;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @PickUp.started += instance.OnPickUp;
                    @PickUp.performed += instance.OnPickUp;
                    @PickUp.canceled += instance.OnPickUp;
                    @Attack.started += instance.OnAttack;
                    @Attack.performed += instance.OnAttack;
                    @Attack.canceled += instance.OnAttack;
                    @Roll.started += instance.OnRoll;
                    @Roll.performed += instance.OnRoll;
                    @Roll.canceled += instance.OnRoll;
                    @Run.started += instance.OnRun;
                    @Run.performed += instance.OnRun;
                    @Run.canceled += instance.OnRun;
                    @DropItem.started += instance.OnDropItem;
                    @DropItem.performed += instance.OnDropItem;
                    @DropItem.canceled += instance.OnDropItem;
                    @ThrowItem.started += instance.OnThrowItem;
                    @ThrowItem.performed += instance.OnThrowItem;
                    @ThrowItem.canceled += instance.OnThrowItem;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @OpenPauseMenu.started += instance.OnOpenPauseMenu;
                    @OpenPauseMenu.performed += instance.OnOpenPauseMenu;
                    @OpenPauseMenu.canceled += instance.OnOpenPauseMenu;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnPickUp(InputAction.CallbackContext context);
            void OnAttack(InputAction.CallbackContext context);
            void OnRoll(InputAction.CallbackContext context);
            void OnRun(InputAction.CallbackContext context);
            void OnDropItem(InputAction.CallbackContext context);
            void OnThrowItem(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnOpenPauseMenu(InputAction.CallbackContext context);
        }
    }
}
